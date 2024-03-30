# 1. PokeTradeBotSV.cs

SysBot.Pokemon/SV/BotTrade/PokeTradeBotSV.cs

## 1.1 ProcessDumpTradeAsync

```csharp
    private async Task<PokeTradeResult> ProcessDumpTradeAsync(PokeTradeDetail<PK9> detail, CancellationToken token)
    {
        int ctr = 0;
        var time = TimeSpan.FromSeconds(Hub.Config.Trade.MaxDumpTradeTime);
        var start = DateTime.Now;

        var pkprev = new PK9();
        var bctr = 0;
        var n = 1;
        Log("Starting ProcessDumpTradeAsync...");
        while (ctr < Hub.Config.Trade.MaxDumpsPerTrade && DateTime.Now - start < time)
        {
            if (!await IsInBox(PortalOffset, token).ConfigureAwait(false))
                break;
            if (bctr++ % 3 == 0)
                await Click(B, 0_100, token).ConfigureAwait(false);

            // Wait for user input... Needs to be different from the previously offered Pokémon.
            var pk = await ReadUntilPresent(TradePartnerOfferedOffset, 3_000, 0_050, BoxFormatSlotSize, token)
                .ConfigureAwait(false);
            if (pk == null || pk.Species < 1 || !pk.ChecksumValid ||
                SearchUtil.HashByDetails(pk) == SearchUtil.HashByDetails(pkprev))
                continue;

            // Save the new Pokémon for comparison next round.
            pkprev = pk;

            // Send results from separate thread; the bot doesn't need to wait for things to be calculated.
            if (DumpSetting.Dump)
            {
                var subfolder = detail.Type.ToString().ToLower();
                DumpPokemon(DumpSetting.DumpFolder, subfolder, pk); // received
            }

            var la = new LegalityAnalysis(pk);
            var verbose = $"```{la.Report(true)}```";
            Log($"Shown Pokémon is: {(la.Valid ? "Valid" : "Invalid")}.");
            //    var la = new LegalityAnalysis(pk);
            //    var verbose = $"```{la.Report(true)}```";
            //    Log($"显示的宝可梦是: {(la.Valid ? "Valid" : "Invalid")}.");

            ctr++;
            var msg = Hub.Config.Trade.DumpTradeLegalityCheck ? verbose : $"File {ctr}";

            // Extra information about trainer data for people requesting with their own trainer data.
            var ot = pk.OriginalTrainerName;
            var ot_gender = pk.OriginalTrainerGender == 0 ? "Male" : "Female";
            var tid = pk.GetDisplayTID().ToString(pk.GetTrainerIDFormat().GetTrainerIDFormatStringTID());
            var sid = pk.GetDisplaySID().ToString(pk.GetTrainerIDFormat().GetTrainerIDFormatStringSID());
            msg += $"\n**Trainer Data**\n```OT: {ot}\nOTGender: {ot_gender}\nTID: {tid}\nSID: {sid}```";

            // Extra information for shiny eggs, because of people dumping to skip hatching.
            var eggstring = pk.IsEgg ? "Egg " : string.Empty;
            msg += pk.IsShiny ? $"\n**This Pokémon {eggstring}is shiny!**" : string.Empty;
            detail.SendNotification(this, pk, msg);
        }

        // Log($"Ended Dump loop after processing {ctr} Pokémon.");
        Log($"在处理{ctr}只宝可梦后结束转储循环。");
        if (ctr == 0)
            return PokeTradeResult.TrainerTooSlow;

        TradeSettings.AddCompletedDumps();
        detail.Notifier.SendNotification(this, detail, $"Dumped {ctr} Pokémon.");
        detail.Notifier.TradeFinished(this, detail, detail.TradeData); // blank PK9
        return PokeTradeResult.Success;
    }

```

> /doc

```csharp
/// <summary>
/// Asynchronously processes a dump trade.
/// </summary>
/// <param name="detail">The trade detail.</param>
/// <param name="token">The cancellation token.</param>
/// <returns>A task that represents the asynchronous operation. The task result contains the trade result.</returns>
private async Task<PokeTradeResult> ProcessDumpTradeAsync(PokeTradeDetail<PK9> detail, CancellationToken token)
{
    int ctr = 0;
    var time = TimeSpan.FromSeconds(Hub.Config.Trade.MaxDumpTradeTime);
    var start = DateTime.Now;

    var pkprev = new PK9();
    var bctr = 0;
    var n = 1;
    Log("Starting ProcessDumpTradeAsync...");
    while (ctr < Hub.Config.Trade.MaxDumpsPerTrade && DateTime.Now - start < time)
    {
        if (!await IsInBox(PortalOffset, token).ConfigureAwait(false))
            break;
        if (bctr++ % 3 == 0)
            await Click(B, 0_100, token).ConfigureAwait(false);

        // Wait for user input... Needs to be different from the previously offered Pokémon.
        var pk = await ReadUntilPresent(TradePartnerOfferedOffset, 3_000, 0_050, BoxFormatSlotSize, token)
            .ConfigureAwait(false);
        if (pk == null || pk.Species < 1 || !pk.ChecksumValid ||
            SearchUtil.HashByDetails(pk) == SearchUtil.HashByDetails(pkprev))
            continue;

        // Save the new Pokémon for comparison next round.
        pkprev = pk;

        // Send results from separate thread; the bot doesn't need to wait for things to be calculated.
        if (DumpSetting.Dump)
        {
            var subfolder = detail.Type.ToString().ToLower();
            DumpPokemon(DumpSetting.DumpFolder, subfolder, pk); // received
        }

        var la = new LegalityAnalysis(pk);
        var verbose = $"```{la.Report(true)}```";
        Log($"Shown Pokémon is: {(la.Valid ? "Valid" : "Invalid")}.");

        ctr++;
        var msg = Hub.Config.Trade.DumpTradeLegalityCheck ? verbose : $"File {ctr}";

        // Extra information about trainer data for people requesting with their own trainer data.
        var ot = pk.OriginalTrainerName;
        var ot_gender = pk.OriginalTrainerGender == 0 ? "Male" : "Female";
        var tid = pk.GetDisplayTID().ToString(pk.GetTrainerIDFormat().GetTrainerIDFormatStringTID());
        var sid = pk.GetDisplaySID().ToString(pk.GetTrainerIDFormat().GetTrainerIDFormatStringSID());
        msg += $"\n**Trainer Data**\n```OT: {ot}\nOTGender: {ot_gender}\nTID: {tid}\nSID: {sid}```";

        // Extra information for shiny eggs, because of people dumping to skip hatching.
        var eggstring = pk.IsEgg ? "Egg " : string.Empty;
        msg += pk.IsShiny ? $"\n**This Pokémon {eggstring}is shiny!**" : string.Empty;
        detail.SendNotification(this, pk, msg);
    }

    Log($"在处理{ctr}只宝可梦后结束转储循环。");
    if (ctr == 0)
        return PokeTradeResult.TrainerTooSlow;

    TradeSettings.AddCompletedDumps();
    detail.Notifier.SendNotification(this, detail, $"Dumped {ctr} Pokémon.");
    detail.Notifier.TradeFinished(this, detail, detail.TradeData); // blank PK9
    return PokeTradeResult.Success;
}
```

## 1.2 ExitTradeToPortal

```csharp
    private async Task ExitTradeToPortal(bool unexpected, CancellationToken token)
    {
        await Task.Delay(1_000, token).ConfigureAwait(false);
        if (await IsInPokePortal(PortalOffset, token).ConfigureAwait(false))
            return;

        if (unexpected)
            // Log("Unexpected behavior, recovering to Portal.");
            Log("意外行为，正在恢复到宝可梦入口站。");

        // Ensure we're not in the box first.
        // Takes a long time for the Portal to load up, so once we exit the box, wait 5 seconds.
        // Log("Leaving the box...");
        Log("正在离开盒子...");
        var attempts = 0;
        while (await IsInBox(PortalOffset, token).ConfigureAwait(false))
        {
            await Click(B, 1_000, token).ConfigureAwait(false);
            if (!await IsInBox(PortalOffset, token).ConfigureAwait(false))
            {
                await Task.Delay(1_000, token).ConfigureAwait(false);
                break;
            }

            await Click(A, 1_000, token).ConfigureAwait(false);
            if (!await IsInBox(PortalOffset, token).ConfigureAwait(false))
            {
                await Task.Delay(1_000, token).ConfigureAwait(false);
                break;
            }

            await Click(B, 1_000, token).ConfigureAwait(false);
            if (!await IsInBox(PortalOffset, token).ConfigureAwait(false))
            {
                await Task.Delay(1_000, token).ConfigureAwait(false);
                break;
            }

            // Didn't make it out of the box for some reason.
            if (++attempts > 20)
            {
                // Log("Failed to exit box, rebooting the game.");
                Log("无法离开盒子，正在重启游戏。");
                if (!await RecoverToOverworld(token).ConfigureAwait(false))
                    await RestartGameSV(token).ConfigureAwait(false);
                await ConnectAndEnterPortal(token).ConfigureAwait(false);
                return;
            }
        }

```

> doc

```csharp
/// <summary>
/// Exits the trade and returns to the Pokémon Portal.
/// </summary>
/// <param name="unexpected">Indicates if the exit was unexpected.</param>
/// <param name="token">The cancellation token.</param>
/// <returns>A task that represents the asynchronous operation.</returns>
private async Task ExitTradeToPortal(bool unexpected, CancellationToken token)
{
    await Task.Delay(1_000, token).ConfigureAwait(false);
    if (await IsInPokePortal(PortalOffset, token).ConfigureAwait(false))
        return;

    if (unexpected)
        Log("意外行为，正在恢复到宝可梦入口站。");

    // Ensure we're not in the box first.
    Log("正在离开盒子...");
    var attempts = 0;
    while (await IsInBox(PortalOffset, token).ConfigureAwait(false))
    {
        await Click(B, 1_000, token).ConfigureAwait(false);
        if (!await IsInBox(PortalOffset, token).ConfigureAwait(false))
        {
            await Task.Delay(1_000, token).ConfigureAwait(false);
            break;
        }

        await Click(A, 1_000, token).ConfigureAwait(false);
        if (!await IsInBox(PortalOffset, token).ConfigureAwait(false))
        {
            await Task.Delay(1_000, token).ConfigureAwait(false);
            break;
        }

        await Click(B, 1_000, token).ConfigureAwait(false);
        if (!await IsInBox(PortalOffset, token).ConfigureAwait(false))
        {
            await Task.Delay(1_000, token).ConfigureAwait(false);
            break;
        }

        // Didn't make it out of the box for some reason.
        if (++attempts > 20)
        {
            Log("无法离开盒子，正在重启游戏。");
            if (!await RecoverToOverworld(token).ConfigureAwait(false))
                await RestartGameSV(token).ConfigureAwait(false);
            await ConnectAndEnterPortal(token).ConfigureAwait(false);
            return;
        }
    }

    // Wait for the portal to load.
    Log("正在等待宝可梦入口站加载...");
    attempts = 0;
    while (!await IsInPokePortal(PortalOffset, token).ConfigureAwait(false))
    {
        await Task.Delay(1_000, token).ConfigureAwait(false);
        if (await IsInPokePortal(PortalOffset, token).ConfigureAwait(false))
            break;

        // Didn't make it into the portal for some reason.
        if (++attempts > 40)
        {
            Log("无法加载宝可梦入口站，正在重启游戏。");
            if (!await RecoverToOverworld(token).ConfigureAwait(false))
                await RestartGameSV(token).ConfigureAwait(false);
            await ConnectAndEnterPortal(token).ConfigureAwait(false);
            return;
        }
    }
}

```

## 1.3 SetBoxPkmWithSwappedIDDetailsSV

```csharp
   private async Task<bool> SetBoxPkmWithSwappedIDDetailsSV(PK9 toSend, TradeMyStatus tradePartner, SAV9SV sav,
        CancellationToken token)
    {
        if (toSend.Species == (ushort)Species.Ditto)
        {
            Log($"Do nothing to trade Pokemon, since pokemon is Ditto");
            return false;
        }

        var cln = toSend.Clone();
        cln.OriginalTrainerGender = (byte)tradePartner.Gender;
        cln.TrainerTID7 = (uint)Math.Abs(tradePartner.DisplayTID);
        cln.TrainerSID7 = (uint)Math.Abs(tradePartner.DisplaySID);
        cln.Language = tradePartner.Language;
        cln.OriginalTrainerName = tradePartner.OT;

        // copied from https://github.com/Wanghaoran86/TransFireBot/commit/f7c5b39ce2952818177a97babb8b3df027e673fb
        ushort species = toSend.Species;
        GameVersion version;
        switch (species)
        {
            case (ushort)Species.Koraidon:
            case (ushort)Species.GougingFire:
            case (ushort)Species.RagingBolt:
                version = GameVersion.SL;
                Log("朱版本限定宝可梦，强制修改版本为朱");
                break;
            case (ushort)Species.Miraidon:
            case (ushort)Species.IronCrown:
            case (ushort)Species.IronBoulder:
                version = GameVersion.VL;
                Log("紫版本限定宝可梦，强制修改版本为紫");
                break;
            default:
                version = (GameVersion)tradePartner.Game;
                break;
        }

        cln.Version = version;

        cln.ClearNickname();

        // thanks @Wanghaoran86
        if (toSend.MetLocation == Locations.TeraCavern9 && toSend.IsShiny)
        {
            cln.PID = (((uint)(cln.TID16 ^ cln.SID16) ^ (cln.PID & 0xFFFF) ^ 1u) << 16) | (cln.PID & 0xFFFF);
        }
        else if (toSend.IsShiny)
        {
            cln.SetShiny();
        }

        cln.RefreshChecksum();

        var tradeSV = new LegalityAnalysis(cln);
        if (tradeSV.Valid)
        {
            Log($"Pokemon is valid, use trade partnerInfo");
            await SetBoxPokemonAbsolute(BoxStartOffset, cln, token, sav).ConfigureAwait(false);
        }
        else
        {
            Log($"Pokemon not valid, do nothing to trade Pokemon");
        }

        return tradeSV.Valid;
    }
```

> doc

```csharp
/// <summary>
/// Sets the box Pokémon with swapped ID details.
/// </summary>
/// <param name="toSend">The Pokémon to send.</param>
/// <param name="tradePartner">The trade partner's status.</param>
/// <param name="sav">The current save file.</param>
/// <param name="token">The cancellation token.</param>
/// <returns>A task that represents the asynchronous operation. The task result contains a boolean indicating if the operation was successful.</returns>
private async Task<bool> SetBoxPkmWithSwappedIDDetailsSV(PK9 toSend, TradeMyStatus tradePartner, SAV9SV sav,
    CancellationToken token)
{
    // If the Pokémon to send is a Ditto, do nothing and return false.
    if (toSend.Species == (ushort)Species.Ditto)
    {
        Log($"Do nothing to trade Pokemon, since pokemon is Ditto");
        return false;
    }

    // Clone the Pokémon to send.
    var cln = toSend.Clone();
    // Set the cloned Pokémon's original trainer gender, TID, SID, language, and OT name to match the trade partner's.
    cln.OriginalTrainerGender = (byte)tradePartner.Gender;
    cln.TrainerTID7 = (uint)Math.Abs(tradePartner.DisplayTID);
    cln.TrainerSID7 = (uint)Math.Abs(tradePartner.DisplaySID);
    cln.Language = tradePartner.Language;
    cln.OriginalTrainerName = tradePartner.OT;

    // Determine the game version based on the species of the Pokémon to send.
    ushort species = toSend.Species;
    GameVersion version;
    switch (species)
    {
        case (ushort)Species.Koraidon:
        case (ushort)Species.GougingFire:
        case (ushort)Species.RagingBolt:
            version = GameVersion.SL;
            Log("朱版本限定宝可梦，强制修改版本为朱");
            break;
        case (ushort)Species.Miraidon:
        case (ushort)Species.IronCrown:
        case (ushort)Species.IronBoulder:
            version = GameVersion.VL;
            Log("紫版本限定宝可梦，强制修改版本为紫");
            break;
        default:
            version = (GameVersion)tradePartner.Game;
            break;
    }

    // Set the cloned Pokémon's version.
    cln.Version = version;

    // Clear the cloned Pokémon's nickname.
    cln.ClearNickname();

    // If the Pokémon to send is shiny and was met in TeraCavern9, adjust its PID.
    if (toSend.MetLocation == Locations.TeraCavern9 && toSend.IsShiny)
    {
        cln.PID = (((uint)(cln.TID16 ^ cln.SID16) ^ (cln.PID & 0xFFFF) ^ 1u) << 16) | (cln.PID & 0xFFFF);
    }
    // If the Pokémon to send is shiny, set the cloned Pokémon to be shiny.
    else if (toSend.IsShiny)
    {
        cln.SetShiny();
    }

    // Refresh the cloned Pokémon's checksum.
    cln.RefreshChecksum();

    // Perform a legality analysis on the cloned Pokémon.
    var tradeSV = new LegalityAnalysis(cln);
    // If the cloned Pokémon is valid, set it as the Pokémon in the box and return true.
    if (tradeSV.Valid)
    {
        Log($"Pokemon is valid, use trade partnerInfo");
        await SetBoxPokemonAbsolute(BoxStartOffset, cln, token, sav).ConfigureAwait(false);
    }
    // If the cloned Pokémon is not valid, do nothing and return false.
    else
    {
        Log($"Pokemon not valid, do nothing to trade Pokemon");
    }

    return tradeSV.Valid;
}
```

```csharp
        if (token.IsCancellationRequested)
        {
            StartFromOverworld = true;
            LastTradeDistributionFixed = false;
            await ExitTradeToPortal(false, token).ConfigureAwait(false);
            return PokeTradeResult.RoutineCancel;
        }
```

```csharp
// Checks if the cancellation token has been triggered.
// If it has, it resets the bot's state to start from the overworld and not use a fixed distribution code for the next trade.
// It then exits the trade and returns to the Pokémon Portal.
// Finally, it returns a result indicating that the routine was cancelled.
if (token.IsCancellationRequested)
{
    // Sets the bot to start from the overworld in the next trade.
    StartFromOverworld = true;
    // Sets the bot to not use a fixed distribution code in the next trade.
    LastTradeDistributionFixed = false;
    // Exits the trade and returns to the Pokémon Portal.
    await ExitTradeToPortal(false, token).ConfigureAwait(false);
    // Returns a result indicating that the routine was cancelled.
    return PokeTradeResult.RoutineCancel;
}
```

```csharp
                tradeOffered =
                    await ReadUntilChanged(TradePartnerOfferedOffset, lastOffered, 25_000, 0_500, false, true, token)
                        .ConfigureAwait(false);
                if (!tradeOffered)
                {
                    await ExitTradeToPortal(false, token).ConfigureAwait(false);
                    return PokeTradeResult.TrainerTooSlow;
                }
```

> /doc

```csharp
// Attempts to read the data at the TradePartnerOfferedOffset until it changes from the lastOffered data.
// This is done to ensure that the trade partner has offered a new Pokémon for trade.
// The method will attempt this for 25 seconds, checking every 500 milliseconds.
tradeOffered = await ReadUntilChanged(TradePartnerOfferedOffset, lastOffered, 25_000, 0_500, false, true, token)
    .ConfigureAwait(false);

// If the trade partner has not offered a new Pokémon within the allotted time, the method will:
// 1. Exit the trade and return to the Pokémon Portal by calling the ExitTradeToPortal method.
// 2. Return a PokeTradeResult indicating that the trainer was too slow to offer a Pokémon.
if (!tradeOffered)
{
    await ExitTradeToPortal(false, token).ConfigureAwait(false);
    return PokeTradeResult.TrainerTooSlow;
}
```

