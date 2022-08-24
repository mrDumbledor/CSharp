/* Murad Aghayev, I. ročník
Letni semestr 2020/2021
Programování NPRG031
*/
using System;
using System.IO;
using System.ComponentModel;
using System.Windows.Forms;
using System.Drawing;

// hlavni menu
class StartScreen : Form {

    static public string name_of_hero = ""; //odpovi za slozitost hry, littlemouse - easy, mouse - hard

    // prejit ke forme s hrou urcite slozitosti 
    void click(string filename) {
        name_of_hero = filename + ".png";        
        Form game = new MyForm();
        game.ShowDialog();
        Application.Restart();
    }

    public StartScreen() {

        ClientSize = new Size(1080, 608); 
        BackgroundImage = Image.FromFile (Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "menu.jpg"));
        
        //nastaveni prvniho tlacitka pro prechod do jednoduche hry
        Button a = new Button();
        a.Text = "EASY";
        a.Width = 400;
        a.Height = 40;
        a.Top = 25;
        a.Left = 25;
        a.BackColor = Color.Peru;
        a.Font = new Font("Futura", 15F);
        a.ForeColor = Color.Black;
        a.FlatStyle = FlatStyle.Popup;
        Controls.Add(a);
        a.Click += new EventHandler((sender, events) => click("littlemouse"));

        //nastaveni prvniho tlacitka pro prechod do slozite hry
        Button b = new Button();
        b.Text = "HARD";
        b.Width = 400;
        b.Height = 40;
        b.Top = 90;
        b.Left = 25;
        b.BackColor = Color.Peru;
        b.Font = new Font("Futura", 15F);
        b.ForeColor = Color.Black;
        b.FlatStyle = FlatStyle.Popup;
        Controls.Add(b);
        b.Click += new EventHandler((sender, events) => click("mouse"));

        // nastaveni nadpisu "Instructions"
        Label Instructions = new Label();
        Instructions.AutoSize = true;
        Instructions.Font = new Font("Futura", 30F, FontStyle.Bold);
        Instructions.ForeColor = Color.White;
        Instructions.BackColor = System.Drawing.Color.ForestGreen;
        Instructions.Left = 100;
        Instructions.Top = 200;
        Instructions.Text = "Instructions:";
        Controls.Add(Instructions);

        // nastaveni textu s instrukci
        TextBox Text = new TextBox();
        Text.Size = new Size(400, 200);
        Text.Left = 25;
        Text.Top = 300;
        Text.Font = new Font("Futura", 20F, FontStyle.Bold);
        Text.Multiline = true;
        Text.ReadOnly = true;
        Text.BackColor = Color.White;
        Text.ForeColor = Color.DarkGreen;
        Text.AutoSize = true;
        Text.Text = "1. Press ENTER to start a new game.\r\n2. Press SPACE to shoot.\r\n3. Press UP to jump.";
        Controls.Add(Text);
    }
}

// forma se samotnou hrou
class MyForm : Form {

    // najit directory filu 
    string filename(string name) =>
        Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), name);

    // obdelnik, ktery je mensi nez obrazek
    // aby zajistit vic tolerantni podminky prohry
    Rectangle mouseBounds => Rectangle.Inflate(mouse.Bounds, -10, -10);
    Rectangle catBounds => Rectangle.Inflate(cat.Bounds, -5, -10);
    Rectangle column1Bounds => Rectangle.Inflate(column1.Bounds, -5, -5);
    Rectangle column2Bounds => Rectangle.Inflate(column2.Bounds, -5, -5);

    static PictureBox column1 = new PictureBox();
    static PictureBox column2 = new PictureBox();
    static PictureBox mouse = new PictureBox();
    static PictureBox cat = new PictureBox();
    static IContainer components = new Container();
    static Timer timer = new Timer(components);
    static Label score = new Label();
    
    Random random = new Random();

    bool game_stop; // status hry
    bool shooting; // status strelby
    int actual_score = 0;
    int previous_score = 0;
    int speedColumn = 11; // rychlost se kterou prichazeji prihradky
    int speedCat = 11; // rychlost se kterou prichazeji kocoury
    int whichCat = 0; // vyber obrazku kocoura
    int gravity = 12; // rychlost se kterou se snizuje mys

    public MyForm() {

        // pokud slozitost hry je vysoka, tak zvetsit rychlosti
          if (StartScreen.name_of_hero == "mouse.png"){
            speedColumn = 20;
            speedCat = 21;
            gravity = 17;
        }

        // nastaveni kocoura
        cat.ImageLocation = filename("tom.png");
        cat.Location = new Point(1000, 200);
        cat.Size = new Size(150, 180);
        cat.BackColor = Color.Transparent;
        cat.SizeMode = PictureBoxSizeMode.StretchImage;

        // nastaveni prihradek
        column1.ImageLocation = filename("column.png");
        column1.Location = new Point(1000, 0);
        column1.Size = new Size(70, 170);
        column1.BackColor = Color.Transparent;
        column1.SizeMode = PictureBoxSizeMode.StretchImage;
        column2.ImageLocation = filename("column.png");
        column2.Location = new Point(500, 330);
        column2.Size = new Size(70, 170);
        column2.BackColor = Color.Transparent;
        column2.SizeMode = PictureBoxSizeMode.StretchImage;

        // nastaveni mysi
        mouse.ImageLocation = filename(StartScreen.name_of_hero);
        mouse.Location = new Point(100, 100);
        mouse.Size = new Size(70, 120);
        mouse.BackColor = Color.Transparent;
        mouse.SizeMode = PictureBoxSizeMode.StretchImage;
        
        // jine nastaveni rozmeru mysi pro jinou slozitost -> jiny obrazek mysi
        if (StartScreen.name_of_hero == "mouse.png"){
            mouse.Size = new Size(140, 120);
        }

        timer.Enabled = true;
        timer.Interval = 25;
        timer.Tick += new System.EventHandler(MainEvents);

        // nastaveni pocitadla
        score.AutoSize = true;
        score.Font = new Font("Futura", 40F);
        score.ForeColor = Color.Black;
        score.BackColor = System.Drawing.Color.White;
        score.Location = new Point(10, 13);
        score.Text = "0";

        BackgroundImage = Image.FromFile (filename("room.jpg"));
        BackgroundImageLayout = ImageLayout.Stretch;
        ClientSize = new Size(1000, 500);
        Text = "Tom & Jerry";

        KeyUp += new KeyEventHandler(KeyIsUp);
        KeyDown += new KeyEventHandler(KeyIsDown);

        Controls.Add(score);
        Controls.Add(column2);
        Controls.Add(column1);
        Controls.Add(mouse);
        Controls.Add(cat);
  }
    // odpovida za hlavni zmeny v prubehu hry
    void MainEvents(object sender, EventArgs events) {

        score.Text = $"{actual_score}"; //obnovit skore

        mouse.Top += gravity; //mys se snizuje dolu

        // aby mys nezmiznula dolu
        if (mouse.Top > 380){
            mouse.Top = 380;
        }

        // vyber noveho kocoura, pokud minuly zmizel
        if (cat.Left < 0) {
            ChooseCat();
        }
        // prohra v pripade srazky kocoura s mysi
        if (mouseBounds.IntersectsWith(catBounds)) {
            EndOfGame();
        }
        cat.Left -= speedCat; // pohyb kocoura doleva

        // pohyb prihradky doleva
        column1.Left -= speedColumn; 
        column2.Left -= speedColumn;

        //vyber nove prihradky nahodne velikosti, pokud minula prihradka zmizela
        if (column1.Left + 170 < 0) {
            column1.Left = 1000;
            column1.Size = new Size(70, random.Next(170, 300));
        }
        if (column2.Left + 170 < 0) {
            column2.Left = 1000;
            column2.Size = new Size(70, random.Next(170, 300));
            column2.Location = new Point(1000, 500 - column2.Height);
        }

        // prohra v pripade srazky prihradky s mysi
        if (mouseBounds.IntersectsWith(column1Bounds) || mouseBounds.IntersectsWith(column2Bounds)) {
            EndOfGame();
        }

        // kontroluje kulky a strelbu 
        foreach (Control x in Controls) {
            if ( (string) x.Tag == "bullet") {

                x.Left += 100;

                if (x.Left > 1000) {
                    BulletFalse( (PictureBox) x);
                }

                if (cat.Bounds.IntersectsWith(x.Bounds)) {
                    BulletFalse( (PictureBox) x);
                    actual_score += 1;
                    ChooseCat();
                }
            }
        }

        // zvetseni rychlosti se zvetsenim skore
        if (actual_score - previous_score >= 4){
            speedColumn += 3;
            speedCat += 3;
            previous_score = actual_score;
        }

    }

    // pokud je stisknuto nejake tlacitko
    void KeyIsDown(object sender, KeyEventArgs button) {

        // pokud je stisknuta mezera, tak mys bude strilet
        if (button.KeyCode == Keys.Space && shooting == false) {
            BulletTrue();
            shooting = true;
        }

        // pokud je stisknuto tlacitko "UP" a slozitost hry je jednoducha, tak mys poskoci mirne
        if (button.KeyCode == Keys.Up && StartScreen.name_of_hero == "littlemouse.png") {
            gravity = -12;
        }
        // pokud je stisknuto tlacitko "UP" a slozitost hry je vysoka, tak mys poskoci silne
        else if (button.KeyCode == Keys.Up && StartScreen.name_of_hero == "mouse.png"){
            gravity = -17;
        }

        // pokud je stisknuto tlacitko "Enter", tak zacne nova hra
        if (game_stop == true && button.KeyCode == Keys.Enter) {
            NewGame();
        }

    }

    // pokud neni stisknuto nejake tlacitko
    void KeyIsUp(object sender, KeyEventArgs button) {

        // pokud slozitost hry je jednoducha, tak mys se bude snizovat mirne
        if (button.KeyCode == Keys.Up && StartScreen.name_of_hero == "littlemouse.png") {
            gravity = 12;
        }
        // pokud slozitost hry je vysoka, tak mys se bude snizovat rychle
        else if (button.KeyCode == Keys.Up && StartScreen.name_of_hero == "mouse.png"){
            gravity = 17;
        }

        shooting = false; // mezera neni stisknuta -> prerusit strelbu

    }

    void NewGame() {
        // nova hra, vsechny promenny nastavit na puvodni hodnoty
        shooting = false; 
        game_stop = false;
        actual_score = 0; 
        speedColumn = 11; 
        speedCat = 11;
        if (StartScreen.name_of_hero == "mouse.png"){
            speedColumn = 20;
            speedCat = 21;
            gravity = 17;
        }
        score.Text = $"{actual_score}";
        column1.Left = 1000; column2.Left = 500;
        timer.Start();
        ChooseCat();
    }

    void EndOfGame() {
        //prohra
        score.Text = $"{actual_score}";
        game_stop = true;
        timer.Stop();
    }

    void BulletFalse(PictureBox bullet) {
        // strelba je prerusena 
        Controls.Remove(bullet); 
    }

    void BulletTrue() {
        // nastaveni kulek
        PictureBox bullet = new PictureBox();
        bullet.BackColor = Color.White;
        bullet.Height = 5; bullet.Width = 10;
        bullet.Left = mouse.Right; 
        bullet.Top = mouse.Top + mouse.Height / 2;
        bullet.Tag = "bullet";
        Controls.Add(bullet);

    }

    void ChooseCat() {

        // nahodny vyber polohy noveho kocoura a zmena obrazku kocoura
        whichCat = (whichCat + 1) % 3;
        cat.Left = 1100;
        cat.Top = random.Next(500 - cat.Height);

        if (whichCat == 0){
            cat.ImageLocation = filename("tom.png");
        }
        else if (whichCat == 1){
            cat.ImageLocation = filename("tom2.png");
        }
        else if (whichCat == 2){
            cat.ImageLocation = filename("tom3.png");
        }
    }

}

class Program {
  static void Main() {
    Form form = new StartScreen();
    form.FormBorderStyle = FormBorderStyle.FixedSingle;
    form.MaximizeBox = false;
    Application.Run(form);
  }
}
