# FileTradeHelper.cs

SysBot.Pokemon/Helpers/FileTradeHelper.cs

### Bin2List

```csharp
        public static List<T> Bin2List(byte[] bb)
        {
            if (pkmSize[typeof(T)] == bb.Length)
            {
                var tp = GetPKM(bb);
                if (tp != null && tp.Species > 0 && tp.Valid && tp is T pkm) return new List<T>() { pkm };
            }
            int size = pkmSizeInBin[typeof(T)];
            int times = bb.Length % size == 0 ? (bb.Length / size) : (bb.Length / size + 1);
            List<T> pkmBytes = new();
            for (var i = 0; i < times; i++)
            {
                int start = i * size;
                int end = (start + size) > bb.Length ? bb.Length : (start + size);
                var tp = GetPKM(bb[start..end]);
                if (tp != null && tp.Species > 0 && tp.Valid && tp is T pkm) pkmBytes.Add(pkm);
            }
            return pkmBytes;
        }
```

```csharp
/// <summary>
/// Converts a byte array from a .bin file to a list of PKM objects.
/// </summary>
/// <param name="bb">The byte array to convert.</param>
/// <returns>A list of PKM objects.</returns>
public static List<T> Bin2List(byte[] bb)
{
    // If the byte array length matches the size of the PKM type, attempt to convert the byte array to a PKM object
    if (pkmSize[typeof(T)] == bb.Length)
    {
        var tp = GetPKM(bb);
        // If the conversion is successful and the PKM object is valid, return a list containing the single PKM object
        if (tp != null && tp.Species > 0 && tp.Valid && tp is T pkm) return new List<T>() { pkm };
    }
    // Get the size of the PKM type in the .bin file
    int size = pkmSizeInBin[typeof(T)];
    // Calculate the number of PKM objects in the byte array
    int times = bb.Length % size == 0 ? (bb.Length / size) : (bb.Length / size + 1);
    // Initialize a list to store the PKM objects
    List<T> pkmBytes = new();
    // Loop through the byte array and convert each segment to a PKM object
    for (var i = 0; i < times; i++)
    {
        // Calculate the start and end indices for the current segment
        int start = i * size;
        int end = (start + size) > bb.Length ? bb.Length : (start + size);
        // Convert the current segment to a PKM object
        var tp = GetPKM(bb[start..end]);
        // If the conversion is successful and the PKM object is valid, add it to the list
        if (tp != null && tp.Species > 0 && tp.Valid && tp is T pkm) pkmBytes.Add(pkm);
    }
    // Return the list of PKM objects
    return pkmBytes;
}
```

The provided C# code is a method named `Bin2List` that belongs to a generic class `FileTradeHelper<T>`. This method is
designed to convert a byte array, which is typically read from a .bin file, into a list of objects of type `T`,
where `T` is a subtype of `PKM`.

The method starts by checking if the length of the byte array matches the size of the `PKM` type `T`. This is done with
the line `if (pkmSize[typeof(T)] == bb.Length)`. If the length matches, it attempts to convert the entire byte array
into a single `PKM` object using the `GetPKM(bb)` method. If the conversion is successful and the resulting `PKM` object
is valid, it returns a list containing this single `PKM` object.

If the length of the byte array does not match the size of the `PKM` type `T`, the method assumes that the byte array
contains multiple `PKM` objects. It calculates the size of each `PKM` object in the .bin file
with `int size = pkmSizeInBin[typeof(T)];` and the number of `PKM` objects in the byte array
with `int times = bb.Length % size == 0 ? (bb.Length / size) : (bb.Length / size + 1);`.

The method then initializes a list to store the `PKM` objects and loops through the byte array. In each iteration, it
calculates the start and end indices for the current segment of the byte array, converts this segment into a `PKM`
object, and if the conversion is successful and the `PKM` object is valid, adds it to the list.

Finally, the method returns the list of `PKM` objects. If the byte array could not be successfully converted into `PKM`
objects, the returned list will be empty.

# AbstractTrade.cs

SysBot.Pokemon/Helpers/AbstractTrade.cs

### StartTradeMultiPKM
```csharp
        public void StartTradeMultiPKM(List<T> rawPkms)
        {
            if (!JudgeMultiNum(rawPkms.Count)) return;

            List<T> pkms = new();
            List<bool> skipAutoOTList = new();
            int invalidCount = 0;
            for (var i = 0; i < rawPkms.Count; i++)
            {
                var _ = CheckPkm(rawPkms[i], out var msg);
                if (!_)
                {
                    LogUtil.LogInfo($"批量第{i + 1}只宝可梦有问题:{msg}", nameof(AbstractTrade<T>));
                    invalidCount++;
                }
                else
                {
                    LogUtil.LogInfo($"批量第{i + 1}只:{GameInfo.GetStrings("zh").Species[rawPkms[i].Species]}", nameof(AbstractTrade<T>));
                    skipAutoOTList.Add(false);
                    pkms.Add(rawPkms[i]);
                }
            }

            if (!JudgeInvalidCount(invalidCount, rawPkms.Count)) return;

            var code = queueInfo.GetRandomTradeCode();
            var __ = AddToTradeQueue(pkms, code, skipAutoOTList,
                PokeRoutineType.LinkTrade, out string message);
            SendMessage(message);
        }
```

```csharp
/// <summary>
/// Starts a multi-trade process with a list of PKM objects.
/// </summary>
/// <param name="rawPkms">The list of raw PKM objects to be traded.</param>
public void StartTradeMultiPKM(List<T> rawPkms)
{
    // Check if the number of PKM objects is within the allowed limit for a multi-trade
    if (!JudgeMultiNum(rawPkms.Count)) return;

    // Initialize a list to store the valid PKM objects and a list to store the skipAutoOT flags
    List<T> pkms = new();
    List<bool> skipAutoOTList = new();
    // Initialize a counter for the number of invalid PKM objects
    int invalidCount = 0;
    // Loop through the raw PKM objects
    for (var i = 0; i < rawPkms.Count; i++)
    {
        // Check if the current PKM object is valid
        var _ = CheckPkm(rawPkms[i], out var msg);
        if (!_)
        {
            // If the PKM object is invalid, log a message and increment the invalid counter
            LogUtil.LogInfo($"批量第{i + 1}只宝可梦有问题:{msg}", nameof(AbstractTrade<T>));
            invalidCount++;
        }
        else
        {
            // If the PKM object is valid, log a message, add a false flag to the skipAutoOT list, and add the PKM object to the valid list
            LogUtil.LogInfo($"批量第{i + 1}只:{GameInfo.GetStrings("zh").Species[rawPkms[i].Species]}", nameof(AbstractTrade<T>));
            skipAutoOTList.Add(false);
            pkms.Add(rawPkms[i]);
        }
    }

    // Check if the number of invalid PKM objects is within the allowed limit
    if (!JudgeInvalidCount(invalidCount, rawPkms.Count)) return;

    // Generate a random trade code
    var code = queueInfo.GetRandomTradeCode();
    // Add the valid PKM objects to the trade queue
    var __ = AddToTradeQueue(pkms, code, skipAutoOTList,
        PokeRoutineType.LinkTrade, out string message);
    // Send a message to notify the user about the trade
    SendMessage(message);
}
```

The provided C# code is a method named `StartTradeMultiPKM` that belongs to an abstract class `AbstractTrade<T>`,
where `T` is a subtype of `PKM`. This method is designed to initiate a multi-trade process with a list of `PKM` objects.

The method begins by checking if the number of `PKM` objects in the list is within the allowed limit for a multi-trade.
This is done with the line `if (!JudgeMultiNum(rawPkms.Count)) return;`. If the number of `PKM` objects exceeds the
limit, the method returns immediately and the multi-trade process is not initiated.

Next, the method initializes a list to store the valid `PKM` objects (`List<T> pkms = new();`) and a list to store
the `skipAutoOT` flags (`List<bool> skipAutoOTList = new();`). It also initializes a counter for the number of
invalid `PKM` objects (`int invalidCount = 0;`).

The method then loops through the raw `PKM` objects. In each iteration, it checks if the current `PKM` object is valid
using the `CheckPkm(rawPkms[i], out var msg);` method. If the `PKM` object is invalid, it logs a message and increments
the invalid counter. If the `PKM` object is valid, it logs a message, adds a false flag to the `skipAutoOT` list, and
adds the `PKM` object to the valid list.

After looping through all the `PKM` objects, the method checks if the number of invalid `PKM` objects is within the
allowed limit with `if (!JudgeInvalidCount(invalidCount, rawPkms.Count)) return;`. If the number of invalid `PKM`
objects exceeds the limit, the method returns immediately and the multi-trade process is not initiated.

If all checks pass, the method generates a random trade code (`var code = queueInfo.GetRandomTradeCode();`), adds the
valid `PKM` objects to the trade
queue (`var __ = AddToTradeQueue(pkms, code, skipAutoOTList, PokeRoutineType.LinkTrade, out string message);`), and
sends a message to notify the user about the trade (`SendMessage(message);`).


### StratTradeMultiPKMWithoutCheck



