using System;
using System.Net.Http;
using DoDo.Open.Sdk.Models.Bots;
using DoDo.Open.Sdk.Models.ChannelMessages;
using DoDo.Open.Sdk.Models.Events;
using DoDo.Open.Sdk.Models.Messages;
using DoDo.Open.Sdk.Services;
using PKHeX.Core;
using SysBot.Base;
using SysBot.Pokemon.Helpers;

namespace SysBot.Pokemon.Dodo
{
    public class PokemonProcessService<TP> : EventProcessService where TP : PKM, new()
    {
        private readonly OpenApiService _openApiService;
        private static readonly string LogIdentity = "DodoBot";
        private static readonly string Welcome = "at我并尝试对我说：\n皮卡丘\nps代码\n或者直接拖一个文件进来";
        private readonly string _channelId;
        private DodoSettings _dodoSettings;
        private string _botDodoSourceId = default!;

        public PokemonProcessService(OpenApiService openApiService, DodoSettings settings)
        {
            _openApiService = openApiService;
            _channelId = settings.ChannelId;
            _dodoSettings = settings;
        }

        public override void Connected(string message)
        {
            Console.WriteLine($"{message}\n");
        }

        public override void Disconnected(string message)
        {
            Console.WriteLine($"{message}\n");
        }

        public override void Reconnected(string message)
        {
            Console.WriteLine($"{message}\n");
        }

        public override void Exception(string message)
        {
            Console.WriteLine($"{message}\n");
        }

        public override void PersonalMessageEvent<T>(
            EventSubjectOutput<EventSubjectDataBusiness<EventBodyPersonalMessage<T>>> input)
        {
            var eventBody = input.Data.EventBody;

            if (eventBody.MessageBody is MessageBodyText messageBodyText)
            {
                DodoBot<TP>.SendPersonalMessage(eventBody.DodoSourceId, $"你好", eventBody.IslandSourceId);
            }
        }

        public override void ChannelMessageEvent<T>(
            EventSubjectOutput<EventSubjectDataBusiness<EventBodyChannelMessage<T>>> input)
        {
            var eventBody = input.Data.EventBody;
            if (!string.IsNullOrWhiteSpace(_channelId) && eventBody.ChannelId != _channelId) return;

            // Check if the message body is a file
            if (eventBody.MessageBody is MessageBodyFile messageBodyFile)
            {
                // Validate the file size and name
                if (!FileTradeHelper<TP>.ValidFileSize(messageBodyFile.Size ?? 0) ||
                    !FileTradeHelper<TP>.ValidFileName(messageBodyFile.Name))
                {
                    // If the file is invalid, withdraw the process and send a message indicating the file is illegal
                    ProcessWithdraw(eventBody.MessageId);
                    DodoBot<TP>.SendChannelMessage("非法文件", eventBody.ChannelId);
                    return;
                }

                // Create a new HttpClient instance
                using var client = new HttpClient();
                // Download the file and convert it to a byte array
                var downloadBytes = client.GetByteArrayAsync(messageBodyFile.Url).Result; // 将下载的二进制文件(不同版本的PKM文件或者是箱子文件bin)转换成byte数组
                // Convert the byte array to a list of PKM objects
                var pkms = FileTradeHelper<TP>.Bin2List(downloadBytes); // 将二进制文件(不同版本的PKM文件或者是箱子文件bin)转换成对应版本的PKM list
                // Withdraw the process
                ProcessWithdraw(eventBody.MessageId);
                // 如果列表中只有一个PKM对象，开始单个交换。检测PKM对象的合法性
                // If there is only one PKM object in the list, start a single trade.Check the legality of the PKM object
                // if (pkms.Count == 1)
                //     new DodoTrade<TP>(ulong.Parse(eventBody.DodoSourceId), eventBody.Personal.NickName,
                //         eventBody.ChannelId, eventBody.IslandSourceId).StartTradePKM(pkms[0]);
                //
                // 如果列表中有多个PKM对象，开始单个交换。不检测PKM对象的合法性(个人使用，对外的机器人不建议这样使用)
                // 鉴于SysBot的合法性检测落后于PKHeX，所以这里不检测合法性
                // If there is only one PKM object in the list, start a single trade. Do not check the legality of the PKM object
                if (pkms.Count == 1)
                    new DodoTrade<TP>(ulong.Parse(eventBody.DodoSourceId), eventBody.Personal.NickName,
                        eventBody.ChannelId, eventBody.IslandSourceId).StartTradeWithoutCheck(pkms[0]);
                // 如果对箱子文件(bin)进行批量交换的时候需要进行合法性校验,请取消下面注释,并注释掉再下面的else if
                // 批量交换检测合法性使用StartTradeMultiPKM，不检测合法性使用StartTradeMultiPKMWithoutCheck
                // If there are multiple PKM objects in the list and the count is within the maximum limit, start a multi-trade
                // else if (pkms.Count > 1 && pkms.Count <= FileTradeHelper<TP>.MaxCountInBin)
                //     new DodoTrade<TP>(ulong.Parse(eventBody.DodoSourceId), eventBody.Personal.NickName,
                //         eventBody.ChannelId, eventBody.IslandSourceId).StartTradeMultiPKM(pkms);
                /*
                 * If there are multiple PKM objects in the list and the count is within the maximum limit, start a multi-trade
                 * without checking the PKM objects
                 * This is because the PKM objects have already been checked when they were created.
                 * Author:Alexander Jiajiason
                 * Date:2024/03/21
                 * 对bin文件批量交换的时候不需要进行合法性校验
                 */
                else if (pkms.Count > 1 && pkms.Count <= FileTradeHelper<TP>.MaxCountInBin)
                    new DodoTrade<TP>(ulong.Parse(eventBody.DodoSourceId), eventBody.Personal.NickName,
                        eventBody.ChannelId, eventBody.IslandSourceId).StartTradeMultiPKMWithoutCheck(pkms);
                // If the file content is incorrect, send a message indicating the file content is incorrect
                else
                    DodoBot<TP>.SendChannelMessage("文件内容不正确", eventBody.ChannelId);
                return;
            }

            if (eventBody.MessageBody is not MessageBodyText messageBodyText) return;

            var content = messageBodyText.Content;

            LogUtil.LogInfo($"{eventBody.Personal.NickName}({eventBody.DodoSourceId}):{content}", LogIdentity);
            if (_botDodoSourceId == null)
            {
                _botDodoSourceId = _openApiService.GetBotInfo(new GetBotInfoInput()).DodoSourceId;
            }

            if (!content.Contains($"<@!{_botDodoSourceId}>")) return;

            content = content.Substring(content.IndexOf('>') + 1);
            if (typeof(TP) == typeof(PK9) && content.Contains("\n\n") &&
                ShowdownTranslator<TP>.IsPS(content)) // 仅SV支持批量，其他偷懒还没写
            {
                ProcessWithdraw(eventBody.MessageId);
                new DodoTrade<TP>(ulong.Parse(eventBody.DodoSourceId), eventBody.Personal.NickName, eventBody.ChannelId,
                    eventBody.IslandSourceId).StartTradeMultiPs(content.Trim());
                return;
            }
            else if (ShowdownTranslator<TP>.IsPS(content))
            {
                ProcessWithdraw(eventBody.MessageId);
                new DodoTrade<TP>(ulong.Parse(eventBody.DodoSourceId), eventBody.Personal.NickName, eventBody.ChannelId,
                    eventBody.IslandSourceId).StartTradePs(content.Trim());
                return;
            }
            else if (content.Trim().StartsWith("dump"))
            {
                ProcessWithdraw(eventBody.MessageId);
                new DodoTrade<TP>(ulong.Parse(eventBody.DodoSourceId), eventBody.Personal.NickName, eventBody.ChannelId,
                    eventBody.IslandSourceId).StartDump();
                return;
            }
            else if (typeof(TP) == typeof(PK9) && content.Trim().Contains('+')) // 仅SV支持批量，其他偷懒还没写
            {
                ProcessWithdraw(eventBody.MessageId);
                new DodoTrade<TP>(ulong.Parse(eventBody.DodoSourceId), eventBody.Personal.NickName, eventBody.ChannelId,
                    eventBody.IslandSourceId).StartTradeMultiChinesePs(content.Trim());
                return;
            }

            var ps = ShowdownTranslator<TP>.Chinese2Showdown(content);
            if (!string.IsNullOrWhiteSpace(ps))
            {
                LogUtil.LogInfo($"收到命令\n{ps}", LogIdentity);
                ProcessWithdraw(eventBody.MessageId);
                new DodoTrade<TP>(ulong.Parse(eventBody.DodoSourceId), eventBody.Personal.NickName, eventBody.ChannelId,
                    eventBody.IslandSourceId).StartTradePs(ps);
            }
            else if (content.Contains("取消"))
            {
                var result = DodoBot<TP>.Info.ClearTrade(ulong.Parse(eventBody.DodoSourceId));
                DodoBot<TP>.SendChannelAtMessage(ulong.Parse(eventBody.DodoSourceId),
                    $" {GetClearTradeMessage(result)}",
                    eventBody.ChannelId);
            }
            else if (content.Contains("位置"))
            {
                var result = DodoBot<TP>.Info.CheckPosition(ulong.Parse(eventBody.DodoSourceId));
                DodoBot<TP>.SendChannelAtMessage(ulong.Parse(eventBody.DodoSourceId),
                    $" {GetQueueCheckResultMessage(result)}",
                    eventBody.ChannelId);
            }
            else
            {
                DodoBot<TP>.SendChannelMessage($"{Welcome}", eventBody.ChannelId);
            }
        }

        public string GetQueueCheckResultMessage(QueueCheckResult<TP> result)
        {
            if (!result.InQueue || result.Detail is null)
                return "你不在队列里";
            var msg = $"你在第{result.Position}位";
            var pk = result.Detail.Trade.TradeData;
            if (pk.Species != 0)
                msg += $"，交换宝可梦：{ShowdownTranslator<TP>.GameStringsZh.Species[result.Detail.Trade.TradeData.Species]}";
            return msg;
        }

        private static string GetClearTradeMessage(QueueResultRemove result)
        {
            return result switch
            {
                QueueResultRemove.CurrentlyProcessing => "你正在交换中",
                QueueResultRemove.CurrentlyProcessingRemoved => "正在删除",
                QueueResultRemove.Removed => "已删除",
                _ => "你不在队列里",
            };
        }

        private void ProcessWithdraw(string messageId)
        {
            if (_dodoSettings.WithdrawTradeMessage)
            {
                DodoBot<TP>.OpenApiService.SetChannelMessageWithdraw(
                    new SetChannelMessageWithdrawInput() { MessageId = messageId }, true);
            }
        }

        public override void MessageReactionEvent(
            EventSubjectOutput<EventSubjectDataBusiness<EventBodyMessageReaction>> input)
        {
            // Do nothing
        }
    }
}
