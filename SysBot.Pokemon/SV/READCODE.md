# 1. PokeRoutineExecutor9SV

SysBot.Pokemon/SV/PokeRoutineExecutor9SV.cs

## 1.1 ReadBoxPokemon(int box, int slot, CancellationToken token)

```csharp
    public override Task<PK9> ReadBoxPokemon(int box, int slot, CancellationToken token)
    {
        // Shouldn't be reading anything but box1slot1 here. Slots are not consecutive.
        // 不应该读取除box1slot1之外的任何内容。插槽不是连续的。
        var jumps = Offsets.BoxStartPokemonPointer.ToArray();
        return ReadPokemonPointer(jumps, BoxFormatSlotSize, token);
    }
```

> /doc

```csharp
/// <summary>
/// Reads a Pokémon from a specific box and slot in the game.
/// </summary>
/// <param name="box">The box number from which the Pokémon should be read.</param>
/// <param name="slot">The slot number in the box from which the Pokémon should be read.</param>
/// <param name="token">A cancellation token that can be used to cancel the operation.</param>
/// <returns>A task that represents the asynchronous operation. The task result contains the Pokémon that was read.</returns>
/// <remarks>
/// This method is designed to read Pokémon from the first slot of the first box (box1slot1) only.
/// The slots in the game are not consecutive, so reading from other slots may not work as expected.
/// </remarks>
public override Task<PK9> ReadBoxPokemon(int box, int slot, CancellationToken token)
{
    // Shouldn't be reading anything but box1slot1 here. Slots are not consecutive.
    // 不应该读取除box1slot1之外的任何内容。插槽不是连续的。
    var jumps = Offsets.BoxStartPokemonPointer.ToArray();
    return ReadPokemonPointer(jumps, BoxFormatSlotSize, token);
}
```

This is a C# method documentation using XML documentation comments. The `<summary>` tag is used to describe what the
method does. The `<param>` tags are used to describe the method's parameters. The `<returns>` tag is used to describe
the return value of the method. The `<remarks>` tag is used to add additional information about the method.

> /explain

The selected C# code is a method named `ReadBoxPokemon`. This method is part of a larger class that handles operations
related to Pokémon in a game. The method is designed to read a Pokémon from a specific box and slot in the game.

The method takes three parameters: `box`, `slot`, and `token`. The `box` and `slot` parameters represent the box number
and slot number from which the Pokémon should be read. The `token` parameter is a cancellation token that can be used to
cancel the operation.

The first line of the method contains a comment indicating that the method is designed to read Pokémon from the first
slot of the first box (box1slot1) only. This is because the slots in the game are not consecutive, so reading from other
slots may not work as expected.

```csharp
// Shouldn't be reading anything but box1slot1 here. Slots are not consecutive.
```

The method then retrieves the pointer to the start of the Pokémon data in the game's memory. This is done by calling
the `BoxStartPokemonPointer` property of the `Offsets` object and converting the result to an array.

```csharp
var jumps = Offsets.BoxStartPokemonPointer.ToArray();
```

Finally, the method calls the `ReadPokemonPointer` method, passing in the pointer to the Pokémon data, the size of the
data, and the cancellation token. This method reads the Pokémon data from the game's memory and returns it as a `PK9`
object, which represents a Pokémon in the game.

```csharp
return ReadPokemonPointer(jumps, BoxFormatSlotSize, token);
```

In summary, the `ReadBoxPokemon` method is used to read a Pokémon from a specific box and slot in the game, but it is
designed to work specifically with the first slot of the first box.

## 1.2 SetBoxPokemonAbsolute(ulong offset, PK9 pkm, CancellationToken token, ITrainerInfo? sav = null)

```csharp
    public Task SetBoxPokemonAbsolute(ulong offset, PK9 pkm, CancellationToken token, ITrainerInfo? sav = null)
    {
        if (sav != null)
        {
            // Update PKM to the current save's handler data
            pkm.UpdateHandler(sav);
            pkm.RefreshChecksum();
        }

        pkm.ResetPartyStats();
        return SwitchConnection.WriteBytesAbsoluteAsync(pkm.EncryptedBoxData, offset, token);
    }
```

> /doc

```csharp
/// <summary>
/// Sets a Pokémon in a specific box and slot in the game.
/// </summary>
/// <param name="offset">The offset in the game's memory where the Pokémon should be written.</param>
/// <param name="pkm">The Pokémon to be written to the game's memory.</param>
/// <param name="token">A cancellation token that can be used to cancel the operation.</param>
/// <param name="sav">Optional parameter. If provided, the Pokémon's handler data will be updated to match the current save's handler data.</param>
/// <returns>A task that represents the asynchronous operation.</returns>
/// <remarks>
/// This method writes the provided Pokémon to the specified offset in the game's memory.
/// If a save file is provided, the Pokémon's handler data is updated to match the handler data of the current save file.
/// The Pokémon's party stats are reset before it is written to the game's memory.
/// </remarks>
public Task SetBoxPokemonAbsolute(ulong offset, PK9 pkm, CancellationToken token, ITrainerInfo? sav = null)
{
    if (sav != null)
    {
        // Update PKM to the current save's handler data
        pkm.UpdateHandler(sav);
        pkm.RefreshChecksum();
    }

    pkm.ResetPartyStats();
    return SwitchConnection.WriteBytesAbsoluteAsync(pkm.EncryptedBoxData, offset, token);
}
```

This is a C# method documentation using XML documentation comments. The `<summary>` tag is used to describe what the
method does. The `<param>` tags are used to describe the method's parameters. The `<returns>` tag is used to describe
the return value of the method. The `<remarks>` tag is used to add additional information about the method.

> /explain

The selected C# code is a method named `SetBoxPokemonAbsolute`. This method is part of a larger class that handles
operations related to Pokémon in a game. The method is designed to set a Pokémon in a specific box and slot in the game.

The method takes four parameters: `offset`, `pkm`, `token`, and `sav`. The `offset` parameter represents the location in
the game's memory where the Pokémon should be written. The `pkm` parameter is the Pokémon to be written to the game's
memory. The `token` parameter is a cancellation token that can be used to cancel the operation. The `sav` parameter is
an optional parameter that, if provided, will be used to update the Pokémon's handler data to match the current save's
handler data.

```csharp
public Task SetBoxPokemonAbsolute(ulong offset, PK9 pkm, CancellationToken token, ITrainerInfo? sav = null)
```

The method first checks if the `sav` parameter is not null. If it is not null, it updates the Pokémon's handler data to
match the handler data of the current save file and refreshes the Pokémon's checksum.

```csharp
if (sav != null)
{
    // Update PKM to the current save's handler data
    pkm.UpdateHandler(sav);
    pkm.RefreshChecksum();
}
```

Next, the method resets the Pokémon's party stats. This is done to ensure that the Pokémon's stats are in a known state
before it is written to the game's memory.

```csharp
pkm.ResetPartyStats();
```

Finally, the method writes the Pokémon's encrypted box data to the specified offset in the game's memory. This is done
asynchronously, and the method returns a task that represents this operation.

```csharp
return SwitchConnection.WriteBytesAbsoluteAsync(pkm.EncryptedBoxData, offset, token);
```

In summary, the `SetBoxPokemonAbsolute` method is used to set a Pokémon in a specific box and slot in the game, updating
the Pokémon's handler data if necessary, resetting its party stats, and writing it to the game's memory.

## 1.3 PerformLinkCodeTrade(SAV9SV sav, PokeTradeDetail<PK9> poke, CancellationToken token)

### 1.3.2

```csharp
                if (Hub.Config.Dodo.DodoScreenshot)
                {
                    var bytes = await SwitchConnection.Screengrab(token).ConfigureAwait(false) ?? Array.Empty<byte>();
                    var result = GetDodoURL(bytes);
                    poke.SendNotification(this, toSend, result);
                }
```

> /explain

The selected code snippet is part of a larger C# program that appears to be a bot for automating Pokémon trades. This
specific part of the code is responsible for taking a screenshot of the Nintendo Switch's screen and sending it to a
specific URL.

Here's a breakdown of what the code does:

1. The code first checks if the `DodoScreenshot` configuration option is enabled in the `Hub.Config.Dodo` object. This
   is done using an `if` statement. If `DodoScreenshot` is set to `true`, the code inside the `if` block will execute.
   If it's `false`, this part of the code will be skipped.

    ```csharp
    if (Hub.Config.Dodo.DodoScreenshot)
    {
        // Code to execute if DodoScreenshot is true
    }
    ```

2. If `DodoScreenshot` is `true`, the code calls the `Screengrab` method on the `SwitchConnection` object. This method
   presumably takes a screenshot of the current state of the Nintendo Switch's screen and returns it as a byte array. If
   the `Screengrab` method fails and returns `null`, the code uses the null-coalescing operator (`??`) to instead assign
   an empty byte array to the `bytes` variable.

    ```csharp
    var bytes = await SwitchConnection.Screengrab(token).ConfigureAwait(false) ?? Array.Empty<byte>();
    ```

3. The byte array is then passed to the `GetDodoURL` method, which presumably uploads the screenshot to a server and
   returns a URL where the screenshot can be viewed.

    ```csharp
    var result = GetDodoURL(bytes);
    ```

4. Finally, the URL is sent as a notification to the user who initiated the Pokémon trade. This is done using
   the `SendNotification` method on the `poke` object, which represents the current Pokémon trade. The `toSend` variable
   likely represents the Pokémon that the bot is about to send in the trade.

    ```csharp
    poke.SendNotification(this, toSend, result);
    ```

In summary, this code takes a screenshot of the Nintendo Switch's screen, uploads it to a server, and sends the URL of
the uploaded screenshot to the user as a notification. This could be useful for providing visual feedback to the user
about the state of the Pokémon trade.

### 1.3.3

```csharp
 LastTradeDistributionFixed = poke.Type == PokeTradeType.Random && !Hub.Config.Distribution.RandomCode;
```

The selected line of code is part of a larger method that handles the process of performing a Pokémon trade in a game.
This specific line is setting a boolean variable `LastTradeDistributionFixed`.

```csharp
LastTradeDistributionFixed = poke.Type == PokeTradeType.Random && !Hub.Config.Distribution.RandomCode;
```
> /explain

The `LastTradeDistributionFixed` variable is being set to the result of a logical operation that checks two conditions:

1. `poke.Type == PokeTradeType.Random`: This checks if the type of the current trade is "Random". The `poke` object
   represents the details of the current trade, and `Type` is a property of this object that indicates the type of the
   trade.

2. `!Hub.Config.Distribution.RandomCode`: This checks if the `RandomCode` property in the `Distribution` settings of the
   application's configuration (`Hub.Config`) is not set. The `!` operator negates the value of `RandomCode`.

The `&&` operator then combines these two conditions, so `LastTradeDistributionFixed` will be `true` only if the trade
type is "Random" and `RandomCode` is not set in the configuration. This could be used later in the code to determine
specific actions or flow based on whether the last trade was a random distribution with a non-fixed code.

`LastTradeDistributionFixed`变量的值被设置为一个逻辑操作的结果，该操作检查两个条件：

1. `poke.Type == PokeTradeType.Random`：这个检查当前交易的类型是否为"Random"。`poke`对象代表当前交易的详细信息，`Type`是这个对象的一个属性，表示交易的类型。

2. `!Hub.Config.Distribution.RandomCode`：这个检查应用程序配置（`Hub.Config`）中的`Distribution`设置的`RandomCode`属性是否未设置。`!`运算符对`RandomCode`的值进行取反。

然后，`&&`运算符将这两个条件组合在一起，所以只有当交易类型是"Random"并且配置中未设置`RandomCode`时，`LastTradeDistributionFixed`才会为`true`。这个结果可能会在代码的后续部分被用来确定基于最后一次交易是否是非固定代码的随机分发来决定特定的行动或流程。







