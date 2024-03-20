# PokemonProcessService.cs

SysBot.Pokemon.Dodo/PokemonProcessService.cs

```csharp
            if (eventBody.MessageBody is MessageBodyFile messageBodyFile)
            {
                if (!FileTradeHelper<TP>.ValidFileSize(messageBodyFile.Size ?? 0) || !FileTradeHelper<TP>.ValidFileName(messageBodyFile.Name))
                {
                    ProcessWithdraw(eventBody.MessageId);
                    DodoBot<TP>.SendChannelMessage("非法文件", eventBody.ChannelId);
                    return;
                }
                using var client = new HttpClient();
                var downloadBytes = client.GetByteArrayAsync(messageBodyFile.Url).Result;   // 将下载的bin文件转换成byte数组
                var pkms = FileTradeHelper<TP>.Bin2List(downloadBytes);     // 将bin文件转换成对应版本的PKM list
                ProcessWithdraw(eventBody.MessageId);
                if (pkms.Count == 1)
                    new DodoTrade<TP>(ulong.Parse(eventBody.DodoSourceId), eventBody.Personal.NickName, eventBody.ChannelId, eventBody.IslandSourceId).StartTradePKM(pkms[0]);
                else if (pkms.Count > 1 && pkms.Count <= FileTradeHelper<TP>.MaxCountInBin)
                    new DodoTrade<TP>(ulong.Parse(eventBody.DodoSourceId), eventBody.Personal.NickName, eventBody.ChannelId, eventBody.IslandSourceId).StartTradeMultiPKM(pkms);
                else
                    DodoBot<TP>.SendChannelMessage("文件内容不正确", eventBody.ChannelId);
                return;
            }
```

```csharp
// Check if the message body is a file
if (eventBody.MessageBody is MessageBodyFile messageBodyFile)
{
    // Validate the file size and name
    if (!FileTradeHelper<TP>.ValidFileSize(messageBodyFile.Size ?? 0) || !FileTradeHelper<TP>.ValidFileName(messageBodyFile.Name))
    {
        // If the file is invalid, withdraw the process and send a message indicating the file is illegal
        ProcessWithdraw(eventBody.MessageId);
        DodoBot<TP>.SendChannelMessage("非法文件", eventBody.ChannelId);
        return;
    }
    // Create a new HttpClient instance
    using var client = new HttpClient();
    // Download the file and convert it to a byte array
    var downloadBytes = client.GetByteArrayAsync(messageBodyFile.Url).Result;
    // Convert the byte array to a list of PKM objects
    var pkms = FileTradeHelper<TP>.Bin2List(downloadBytes);
    // Withdraw the process
    ProcessWithdraw(eventBody.MessageId);
    // If there is only one PKM object in the list, start a single trade
    if (pkms.Count == 1)
        new DodoTrade<TP>(ulong.Parse(eventBody.DodoSourceId), eventBody.Personal.NickName, eventBody.ChannelId, eventBody.IslandSourceId).StartTradePKM(pkms[0]);
    // If there are multiple PKM objects in the list and the count is within the maximum limit, start a multi-trade
    else if (pkms.Count > 1 && pkms.Count <= FileTradeHelper<TP>.MaxCountInBin)
        new DodoTrade<TP>(ulong.Parse(eventBody.DodoSourceId), eventBody.Personal.NickName, eventBody.ChannelId, eventBody.IslandSourceId).StartTradeMultiPKM(pkms);
    // If the file content is incorrect, send a message indicating the file content is incorrect
    else
        DodoBot<TP>.SendChannelMessage("文件内容不正确", eventBody.ChannelId);
    return;
}
```

The selected code is part of a larger class `PokemonProcessService<TP>`, where `TP` is a generic type parameter that
extends `PKM`. This class seems to be part of a bot system for the Pokemon game, specifically a bot that interacts with
the Dodo system.

The selected line of code is within the `ChannelMessageEvent<T>` method. This method appears to handle different types
of messages that the bot may receive in a channel. The specific line of code you're asking about is:

```csharp
var pkms = FileTradeHelper<TP>.Bin2List(downloadBytes);
```

This line is part of a block of code that executes when the bot receives a file in a channel message. The file is
downloaded and converted into a byte array (`downloadBytes`). The `Bin2List` method of the `FileTradeHelper<TP>` class
is then used to convert this byte array into a list of Pokemon (`pkms`), where each Pokemon is of the type `TP`.

The `Bin2List` method presumably takes a binary representation of one or more Pokemon (in the form of a byte array) and
converts it into a list of Pokemon objects. The specifics of this conversion would depend on the binary format of the
Pokemon data and the structure of the `TP` class.

After this line, the code checks the number of Pokemon in the list (`pkms.Count`) and performs different actions based
on this count. If there's only one Pokemon, it starts a single Pokemon trade. If there are multiple Pokemon (but not
more than the maximum allowed), it starts a multi-Pokemon trade. If the count is outside these bounds, it sends a
message indicating that the file content is incorrect.







