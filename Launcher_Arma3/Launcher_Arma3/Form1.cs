﻿//|------DO-NOT-REMOVE------|
//
// Creator: HUBERT Léo 
// Site   : Emodyz.com
// Created: 13.Jul.2015
// Changed: 13.Jul.2015
// Version: 1.0.0.0
//
//|------DO-NOT-REMOVE------|


using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Globalization;
using System.Threading;
using System.Net.NetworkInformation;
using System.Xml;
using System.Xml.Linq;

namespace Launcher_Arma3
{
   
    public partial class Launch : Form
    {
        //|------ READ ME------|
        //
        // This source code is translated in " ENGLISH */* FRENCH "
        // Ce code source est traduit en " ANGLAIS */* FRANCAIS "
        //
        //|------ READ ME ------|


        // Configuration base of launcher  */* Configuration de base du launcher 
        const string ftp = "http://play.emodyz.com/dev/";  // WebLink of your FTP server */* Lien web de votre serveur FTP
        const string servername = "Emodyz"; // Name of your server */* Nom de votre serveur 
        const string namelaunch = "Emodyz Launcher ©";  // Name of your launcher */* Nom de votre launcher
        const string modsname = "@Emodyz"; // Patch of your mods directory */* Patch de votre répertoire de mods
        const string website = "http://emodyz.com"; // Link of your web site */* Lien de votre site web
        const string extention = "Emodyz.exe"; // Your sofware extension .exe ( example: "Emodyz.exe ) */* votre logiciel .exe ( exemple: " Emodyz.exe " )

        // Config server */* Config serveur
        const string ipserver = "play.emodyz.com:2302"; // Your Arma3 server ip  */* L'ip de votre serveur Arma 3 
        const string servpassword = "none"; // Password of your arma 3 server ( if you don't have a password make " none " ) */* Le mots de passe de votre serveur Arma 3 ( si vous n'avez pas de mot de passe mettez " none " )

        //Configuration language
        public string language = "FR"; // Make your language "EN" pour l'anglais */* Mettez votre langage "FR" pour le français.

        /*
           List of language // Liste des langues
         
         - English // Anglais  //  " EN "
         - French // Français  //  " FR "
         - German // Allemand  //  " AL "
         
        */

        // Config VocalServer
        const string servervocal = "teamspeak3"; // Choise your serveur vocal ("teamspeak3" or "mumble") */* Choissisez votre serveur vocal ("teamspeak3" ou "mumble")

        const string ipmumble = "none"; // Your Mumble server ip  */* Votre ip serveur mumble
        const string portmumble = "none"; // Your port mumble server ( if don't have a port make " none " ) */* Vos port mumble serveur ( si vous n'avez pas de port mettez " none ")
        const string passmumble = "none"; // Your password mumble server ( if don't have a port make " none " ) */* Votre mots de passe mumble serveur ( si vous n'avez pas de port mettez " none ")

        const string ipTS = "ts3.emodyz.com"; // Your TeamSpeak 3 server */* Votre TeamSpeak 3 Serveur Ip
        const string portTS = "none"; // Your port teamspeak 3 server ( if don't have a port make " none " ) */* Vos port TeamSpeak 3 serveur ( si vous n'avez pas de port mettez " none ")
        const string passTS = "none"; // Your password TeamSpeak3 server ( if don't have a port make " none " ) */* Votre mots de passe TeamSpeak 3 serveur ( si vous n'avez pas de port mettez " none ")

        //Settings destination 
        string dest_version = "version"; // Folder where all version file is located */* Dossier ou sont placé tout les fichier version
        string dest_update = "update"; // Folder where all update launcher file is located */* Dossier ou sont placé tout les fichier de mise à jour du launcher
        string dest_arma = "C:\\Program Files (x86)\\Steam\\SteamApps\\common\\Arma 3\\"; // Folder of arma 3 */* Dossier d'arma 3

        // Settings file */* Paramètre fichier
        string file_vlauncher = "vlauncher.txt"; // Distant file launcher version */* Fichier distant version launcher
        string file_darma = "darma3.a3";  // Local file directory arma 3 */* Fichier local destination arma3
        string file_arma3 = "arma3battleye.exe"; // Extention of Arma3 */* Extention d'arma3
        string file_translate = "translate.xml";

        //Settings Update */* Paramètre mise à jour
        string update_ext = "Update.exe"; // Distant program for update launcher */* Fichier distant pour la mise à jour du launcher
        string update_site = "site.txt"; // Local File where is the website for download the update */* Fichier local là ou est le lien pour télécharger la mise à jour
        string update_destlaunch = "update.txt"; // Local File where is the patch to launcher up to date */* Fichier local là ou est la destination du launcher à mettre à jours
                                                             
        // Parametre anexe 

        string appdata = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)+ "\\" + servername + "\\"; // DON'T CHANGE
        string dlauncher = Application.ExecutablePath; // DON'T CHANGE
        string vlauncher = Application.ProductVersion.ToString();// DON'T CHANGE
        string username = "UserName"; // DON'T CHANGE
        string msg_darma = "Arma3 Directory: ";
        bool connection = NetworkInterface.GetIsNetworkAvailable();// DON'T CHANGE
        bool locked = false; // DON'T CHANGE 



        public Launch()
        {
            InitializeComponent();
        }

        // Launcher Load Script 

        private void Launch_Load(object sender, EventArgs e)
        {

            // Change Launcher Name  */* Change le nom du launcher
            Launch.ActiveForm.Text = namelaunch;
            iTalk_ThemeContainer1.Text = namelaunch;

            //Change language launcher */* Change la langue du launcher
            Change_Lang.RunWorkerAsync();

            // Change le bouton TeamSpeak ou Mumble
            if (servervocal == "teamspeak3")
            {
                Vocal_bouton.Text = "TeamSpeak 3";

            }
            if (servervocal == "mumble")
            {
                Vocal_bouton.Text = "Mumble";
            }

            if (credits_label.Text != Application.CompanyName)
            {
                MessageBox.Show("Credits is modified ! */* Les crédits sont modifié !",  "Copyright HUBERT Léo © 2014 - 2015");
                credits_label.Text = "Copyright HUBERT Léo © 2014 - 2015";
                locked = true;
            }


            // Create AppData repertory */* Crée le répertoire AppData
            if (!Directory.Exists(appdata))
            {
                Directory.CreateDirectory(appdata);
            }


            // Launch BackGround Worker  */* Lance les BackGround Worker
            Update_Launcher.RunWorkerAsync(); // Update background worker */* Mise à jour background worker


            // Load if arma3 destination is completed */* Charge si la destination d'arma3 est fini
            if (File.Exists(appdata + file_darma))
            {
                dest_arma = File.ReadAllText(appdata + file_darma);
            }
            else
            {
                if (!File.Exists(dest_arma + file_arma3))
                {
                    Folder.ShowDialog();
                    dest_arma = Folder.SelectedPath + "\\";
                    File.WriteAllText(appdata + file_darma, dest_arma);
                }
            }

            if (!File.Exists(dest_arma + file_arma3))
            {
                MessageBox.Show("Erreur #401 | Arma3 Directory is not valid, choose a new Directory"
                    + Environment.NewLine + "Erreur #401 | Destination d'Arma3 non valide, choisissez un nouvelle destination"
                    + Environment.NewLine + Environment.NewLine + "Default: C:\\Program Files (x86)\\Steam\\SteamApps\\common\\Arma 3");
                label_darma.ForeColor = Color.Red;
                picture_darma.Image = Properties.Resources.cross;
            }
            else
            {
                label_darma.ForeColor = Color.Green;
                picture_darma.Image = Properties.Resources.checkmark;
            }

            label_darma.Text = msg_darma + dest_arma;

            

        }

        
    // [CREDIT][DO NOT REMOVE]
    //
    // This module was written by HUBERT Léo 
    //
    // Copyright HUBERT Léo © 2014 - 2015
    //
    // [CREDIT][DO NOT REMOVE]

        private void WebSite_bouton_Click(object sender, EventArgs e)
        {
            Process.Start(website);
        }


        private void destination_bouton_Click_1(object sender, EventArgs e)
        {
            Folder.ShowDialog();
            dest_arma = Folder.SelectedPath + "\\";
            File.WriteAllText(appdata + file_darma, dest_arma);
            if (!File.Exists(dest_arma + file_arma3))
            {
                MessageBox.Show("Erreur #401 | Arma3 Directory is not valid, choose a new Directory"
                    + Environment.NewLine + "Erreur #401 | Destination d'Arma3 non valide, choisissez un nouvelle destination"
                    + Environment.NewLine + Environment.NewLine + "Default: C:\\Program Files (x86)\\Steam\\SteamApps\\common\\Arma 3");
                label_darma.ForeColor = Color.Red;
                picture_darma.Image = Properties.Resources.cross;
            }
            else
            {
                label_darma.ForeColor = Color.Green;
                picture_darma.Image = Properties.Resources.checkmark;
            }

            label_darma.Text = msg_darma + dest_arma;

        }


        private void Vocal_bouton_Click(object sender, EventArgs e)
        {
            if (credits_label.Text != Application.CompanyName)
            {
                MessageBox.Show("Credits is modified ! */* Les crédits sont modifié !", "Copyright HUBERT Léo © 2014 - 2015");
                credits_label.Text = "Copyright HUBERT Léo © 2014 - 2015";
            }
            if (servervocal == "teamspeak3")
            {
                if (portTS == "none")
                {
                    if (passTS == "none")
                    {
                        Process.Start("ts3server://" + ipTS);
                    }
                    else
                    {
                        Process.Start("ts3server://" + ipTS + "?password=" + passTS);
                    }
                }
                else
                {
                    if (passTS == "none")
                    {
                        Process.Start("ts3server://" + ipTS + "?port=" + portTS);
                    }
                    else
                    {
                        Process.Start("ts3server://" + ipTS + "?port=" + portTS + "?password=" + passTS);
                    }

                }
            }

            if (servervocal == "mumble")
            {
                if (portmumble == "none")
                {
                    if (passmumble == "none")
                    {
                        Process.Start("mumble://" + ipmumble);
                    }
                    else
                    {
                        Process.Start("mumble://:" + passmumble + "@" + ipmumble);
                    }
                }
                else
                {
                    if (passmumble == "none")
                    {
                        Process.Start("mumble://" + ipmumble + portmumble);
                    }
                    else
                    {
                        Process.Start("mumble://:" + passmumble + "@" + ipmumble + portmumble);
                    }

                }
            }
        }

        private void play_bouton_Click(object sender, EventArgs e)
        {
            if (locked)
            {
                MessageBox.Show("Launcher Bloquer ! "+ Environment.NewLine + Environment.NewLine +"Cause: Changement de crédits .");
                return;
            }
            
            MessageBox.Show("CODAGE IN PROGRESS */* EN COUR DE CODAGE" , namelaunch );
        }

        private void Option_Boutton_Click(object sender, EventArgs e)
        {
            Form2 frm = new Form2(language);
            frm.Show();
         
        }



        private void Update_Launcher_DoWork(object sender, DoWorkEventArgs e)
        {
            WebClient client = new WebClient();
            Stream stream = client.OpenRead(ftp + dest_version + "/" + file_vlauncher );
            StreamReader reader = new StreamReader(stream);
            String content = reader.ReadToEnd();


            if (content != vlauncher)
            {
                // Create Update folder */* crée le dossier Update
                if (!Directory.Exists(appdata + "\\" + dest_update))
                {
                    Directory.CreateDirectory(appdata + "\\" + dest_update);
                }

               // Show a dialog before update */* montre un dialogue avant l'update 
                MessageBox.Show("Une mise à jour du launcher est disponible ! " + Environment.NewLine + Environment.NewLine + "Version Launcher: " + vlauncher + Environment.NewLine + "Version Update: " + content);
                
                //Start the update program */* lance le programme de mise à jour 
                WebClient webClient = new WebClient();
                webClient.DownloadFile(ftp + dest_update + "/" + update_ext, appdata + "\\" + dest_update + "\\" + update_ext);
      
                //Write into file update 
                if (File.Exists(appdata + "\\" + dest_update + "\\" + update_site))
                {
                    File.Delete(appdata + "\\" + dest_update + "\\" + update_site);
                }
                File.AppendAllText(appdata + "\\" + dest_update  + "\\" + update_site,ftp + dest_update + "/" + extention);
                /*
                TextWriter up_site = new StreamWriter(appdata + "\\" + dest_update  + "\\" + update_site);
                up_site.WriteLine(ftp + dest_update + "/" + extention);
                up_site.Close();
                */


                //Write into file update 
                if (File.Exists(appdata + "\\" + dest_update + "\\" + update_destlaunch))
                {
                    File.Delete(appdata + "\\" + dest_update + "\\" + update_destlaunch);
                }
                File.AppendAllText(appdata + "\\" + dest_update + "\\" + update_destlaunch, dlauncher);
                /*
                TextWriter up_destlauncher = new StreamWriter(appdata + "\\" + dest_update + "\\" + update_destlaunch);
                up_destlauncher.WriteLine(dlauncher);
                up_destlauncher.Close();
                */

                Process.Start(appdata + "\\" + dest_update + "\\" + update_ext );
                        
                Application.Exit();

            }


          
            
        }

        private void Change_Lang_DoWork(object sender, DoWorkEventArgs e)
        {

            // Download XML translate */* Télécharge le fichier XML
            if (File.Exists(appdata + file_translate))
            {
                File.Delete(appdata + file_translate);
            }
            WebClient webClient = new WebClient();
            webClient.DownloadFile(ftp + file_translate , appdata + file_translate);

            // Open XML doc */* Ouvre le fichier XML

            XDocument xmlDoc = XDocument.Load(appdata + file_translate);

            // Search translate */* Cherche la traduction
            var tr_link = xmlDoc.Descendants(language).Elements("Links").Select(r => r.Value).ToArray();
            var tr_website = xmlDoc.Descendants(language).Elements("WebSite").Select(r => r.Value).ToArray();
            var tr_play = xmlDoc.Descendants(language).Elements("Play").Select(r => r.Value).ToArray();
            var tr_connected = xmlDoc.Descendants(language).Elements("Connected").Select(r => r.Value).ToArray();
            var tr_disconnected = xmlDoc.Descendants(language).Elements("Disconnected").Select(r => r.Value).ToArray();
            var tr_settings = xmlDoc.Descendants(language).Elements("Settings").Select(r => r.Value).ToArray();
            var tr_directory = xmlDoc.Descendants(language).Elements("Directory").Select(r => r.Value).ToArray();

            string tra_link = string.Join(",", tr_link);
            string tra_website = string.Join(",", tr_website);
            string tra_play = string.Join(",", tr_play);
            string tra_connected = string.Join(",", tr_connected);
            string tra_disconnected = string.Join(",", tr_disconnected);
            string tra_settings = string.Join(",", tr_settings);
            string tra_directory = string.Join(",", tr_directory);




            // Change bouton text */* Change le text des boutons 
            Group_Link.Text = tra_link;
            WebSite_bouton.Text = tra_website;
            Play_bouton.Text = tra_play;
            Option_Boutton.Text = tra_settings;
            destination_bouton.Text = tra_directory;
            msg_darma = tra_directory + " Arma3: ";
            label_darma.Text = msg_darma + dest_arma;

            //Load Connection bouton
            if (connection == true)
            {
                connection_label.ForeColor = Color.Green;
                connection_label.Text = tra_connected;
            }
            else
            {
                connection_label.ForeColor = Color.Red;
                connection_label.Text = tra_disconnected;
            }

     

        }

    }
}



// [CREDIT][DO NOT REMOVE]
//
// This module was written by HUBERT Léo 
//
// Copyright HUBERT Léo © 2014 - 2015
//
// [CREDIT][DO NOT REMOVE]