## First year, summer 2021, Programming 2
**Anotace:** Hra s postavy z animovaného filmu "Tom & Jerry". Uživatel hraje za mys, ktera skaka mezi prihradkami a zaroven strili po kocourech.

# Dokumentace
## Spuštění hry
Windows Forms je defaultne zahrnuto v Mono (Linux) a .Net (Windows).
**Windows:** Vytvořte projekt Windows Forms. Hru můžete spustit tlačítkem Play nebo klávesou F5.
**Linux:** Napište `csc game.cs -r:System.Windows.Forms.dll` a pak `mono game.exe` v terminalu.

## Režímy hry
Hra podporuje 2 režimy "Easy" a "Hard", které se liší rychlosti hry.

## Instrukce
Hlavní podmínkou hry je nedotýkat se do přihrádek a kocouru. Pomoci klávesy SPACE uživatel může střílet po kocourech a dostat za to body. Pomoci klavesy UP uživatel muze skákat mezi přihrádkami. Pokud prohraje, může začít novou hru pomoci klávesy ENTER. Při zvýšeni skóre hry, take se zvětšuje rychlost přihrádek a kocouru.

## Struktura programu
Na začátku uživatel vidí hlavní menu se dvema tlačítky "EASY" a "HARD" a instrukce.Za hlavní menu je zodpovedný `class StartScreen`. Pokud uživatel stiskne jedno z těch tlačítek, tak začne hra.Rozdíl mezi tlačítky je rychlost kocouru a přihrádek a obrazek myší v průbehu hry. Za hru je zodpovedný `class MyForm`, který ma 8 základních metod. `MainEvents` odpovídá za hlavní změny v průběhu hry (například, pohyb myší, přihrádek a kocouru, střelbu a srážky mezi objekty). `KeyIsDown` a `KeyIsUp` odpovída za stisknutí tlačítek a tím vyvolané změny. `New Game` a `EndOfGame` odpoví za začátek a konec hry. `BulletTrue` a `BulletFalse` odpoví za zobrazení kulek. `ChooseCat` odpovi za zobrazení kocouru.
