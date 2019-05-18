# InfiniteKnightsSaveEditor


## Usage (Android)

1. First upload your save data to the cloud from within the game, so that you can restore it if anything goes wrong.

2. Copy save data file from your phone to PC.

   Path: `/sdcard/Android/data/com.lemonjamstudio.infiniteknights.lemonjam/files/gde_mod_data.bytes`

3. Edit save data file with this editor. Example:

   ```
   > dotnet InfiniteKnightsSaveEditor.dll --ads-ticket 1000 --vip 1 -i gde_mod_data.bytes -o modified.bytes
   ```

   See more details: `dotnet InfiniteKnightsSaveEditor.dll --help`

4. Put edited save data file back to your phone. But simply overwriting the original file does not work. You need to do
   the followings:

   1. Clear data for the game app (`App Info` -> `Storage` -> `CLEAR DATA`).
   2. Manually restore the directory structure (create removed directories)
      `/sdcard/Android/data/com.lemonjamstudio.infiniteknights.lemonjam/files`.
   3. Copy edited save data file to
      `/sdcard/Android/data/com.lemonjamstudio.infiniteknights.lemonjam/files/gde_mod_data.bytes`.

5. Now run the game.
