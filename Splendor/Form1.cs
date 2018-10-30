/**
 * \file      frmAddVideoGames.cs
 * \author    F. Andolfatto
 * \version   1.0
 * \date      August 22. 2018
 * \brief     Form to play.
 *
 * \details   This form enables to choose coins or cards to get ressources (precious stones) and prestige points 
 * to add and to play with other players
 */


using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Splendor
{
    /// <summary>
    /// manages the form that enables to play with the Splendor
    /// </summary>
    public partial class frmSplendor : Form
    {
        //used to store the number of coins selected for the current round of game
        private int nbRubis;
        private int nbOnyx;
        private int nbEmeraude;
        private int nbDiamand;
        private int nbSaphir;

        private int id = 0;

        //used to store the total number of coins
        private int totalCoins;

        //used to store the number of coins available
        private int availableDiamand = 7;
        private int availableEmeraude = 7;
        private int availableOnyx = 7;
        private int availableRubis = 7;
        private int availableSaphir = 7;
        private int availableGold = 5;

        //id of the player that is playing
        private int currentPlayerId;
        //boolean to enable us to know if the user can click on a coin or a card
        private bool enableClicLabel;
        //connection to the database
        private ConnectionDB conn;

        /// <summary>
        /// constructor
        /// </summary>
        public frmSplendor()
        {
            InitializeComponent();
        }

        /// <summary>
        /// loads the form and initialize data in it
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmSplendor_Load(object sender, EventArgs e)
        {
            lblGoldCoin.Text = availableGold.ToString();

            lblDiamandCoin.Text = availableDiamand.ToString();
            lblEmeraudeCoin.Text = availableEmeraude.ToString();
            lblOnyxCoin.Text = availableOnyx.ToString();
            lblRubisCoin.Text = availableRubis.ToString();
            lblSaphirCoin.Text = availableSaphir.ToString();

            txtLevel11.ReadOnly = true;
            txtLevel12.ReadOnly = true;
            txtLevel13.ReadOnly = true;
            txtLevel14.ReadOnly = true;
            txtLevel21.ReadOnly = true;
            txtLevel22.ReadOnly = true;
            txtLevel23.ReadOnly = true;
            txtLevel24.ReadOnly = true;
            txtLevel31.ReadOnly = true;
            txtLevel32.ReadOnly = true;
            txtLevel33.ReadOnly = true;
            txtLevel34.ReadOnly = true;
            txtNoble1.ReadOnly = true;
            txtNoble2.ReadOnly = true;
            txtNoble3.ReadOnly = true;
            txtNoble4.ReadOnly = true;

            conn = new ConnectionDB();

            //load cards from the database
            Stack<Card> listCardOne = conn.GetListCardAccordingToLevel(1);
            //Go through the results
            //Don't forget to check when you are at the end of the stack
            
            //fin TO DO

            this.Width = 680;
            this.Height = 540;

            enableClicLabel = false;

            lblChoiceDiamand.Visible = false;
            lblChoiceOnyx.Visible = false;
            lblChoiceRubis.Visible = false;
            lblChoiceSaphir.Visible = false;
            lblChoiceEmeraude.Visible = false;
            cmdValidateChoice.Visible = false;
            cmdNextPlayer.Visible = false;

            //we wire the click on all cards to the same event
            txtLevel11.Click += ClickOnCard;
            txtLevel12.Click += ClickOnCard;
            txtLevel13.Click += ClickOnCard;
            txtLevel14.Click += ClickOnCard;
            txtLevel21.Click += ClickOnCard;
            txtLevel22.Click += ClickOnCard;
            txtLevel23.Click += ClickOnCard;
            txtLevel24.Click += ClickOnCard;
            txtLevel31.Click += ClickOnCard;
            txtLevel32.Click += ClickOnCard;
            txtLevel33.Click += ClickOnCard;
            txtLevel34.Click += ClickOnCard;
            txtNoble1.Click += ClickOnCard;
            txtNoble2.Click += ClickOnCard;
            txtNoble3.Click += ClickOnCard;
            txtNoble4.Click += ClickOnCard;
        }

        private void ClickOnCard(object sender, EventArgs e)
        {
            //We get the value on the card and we split it to get all the values we need (number of prestige points and ressource)
            //Enable the button "Validate"
            //TO DO
        }

        /// <summary>
        /// click on the play button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmdPlay_Click(object sender, EventArgs e)
        {
            this.Width = 680;
            this.Height = 780;
           
            LoadPlayer(id);
        }


        /// <summary>
        /// load data about the current player
        /// </summary>
        /// <param name="id">identifier of the player</param>
        private void LoadPlayer(int id) { 

            enableClicLabel = true;

            string name = conn.GetPlayerName(currentPlayerId);

            //no coins or card selected yet, labels are empty
            lblChoiceDiamand.Text = "";
            lblChoiceOnyx.Text = "";
            lblChoiceRubis.Text = "";
            lblChoiceSaphir.Text = "";
            lblChoiceEmeraude.Text = "";

            lblChoiceCard.Text = "";

            //no coins selected
            nbDiamand = 0;
            nbOnyx = 0;
            nbRubis = 0;
            nbSaphir = 0;
            nbEmeraude = 0;

            Player player = new Player();
            player.Name = name;
            player.Id = id;
            player.Ressources = new int[] { 2, 0, 1, 1, 1 };
            player.Coins = conn.GetPlayerCoins(id);

            lblPlayerRubisCoin.Text = player.Coins[0].ToString();
            lblPlayerEmeraudeCoin.Text = player.Coins[1].ToString();
            lblPlayerOnyxCoin.Text = player.Coins[2].ToString();
            lblPlayerSaphirCoin.Text = player.Coins[3].ToString();
            lblPlayerDiamandCoin.Text = player.Coins[4].ToString();

            currentPlayerId = id;

            lblPlayer.Text = "Jeu de " + name;

            cmdPlay.Enabled = false;
        }

        /// <summary>
        /// click on the red coin (rubis) to tell the player has selected this coin
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lblRubisCoin_Click(object sender, EventArgs e)
        {
            if (enableClicLabel)
            {
                cmdValidateChoice.Visible = true;
                lblChoiceRubis.Visible = true;

                if(availableRubis == 2)
                {
                    MessageBox.Show("Ce type de jeton ne peut plus être retiré!");
                }
                else
                {
                    if (nbRubis == 2 || nbSaphir == 2 || nbOnyx == 2 || nbEmeraude == 2 || nbDiamand == 2)
                    {
                        MessageBox.Show("Vous possédez déjà 2 jetons!");
                    }
                    else
                    {
                        totalCoins = nbSaphir + nbOnyx + nbEmeraude + nbDiamand;

                        if (nbRubis == 1 && totalCoins >= 1)
                        {                                                  
                            MessageBox.Show("Vous ne pouvez avoir qu'un jeton!");                           
                        }
                        else
                        {
                            if(nbRubis + totalCoins != 3)
                            {
                                nbRubis++;
                                availableRubis--;
                                lblRubisCoin.Text = availableRubis.ToString();
                                lblChoiceRubis.Text = nbRubis + "\r\n";
                            }
                            else
                            {
                                MessageBox.Show("Vous avez atteint le nombre de jetons maximum!");
                            }
                        }
                    }
                }                
            }
        }

        /// <summary>
        /// click on the blue coin (saphir) to tell the player has selected this coin
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lblSaphirCoin_Click(object sender, EventArgs e)
        {
            if (enableClicLabel)
            {
                cmdValidateChoice.Visible = true;
                lblChoiceSaphir.Visible = true;

                if (availableSaphir == 2)
                {
                    MessageBox.Show("Ce type de jeton ne peut plus être retiré!");
                }
                else
                {
                    if (nbRubis == 2 || nbSaphir == 2 || nbOnyx == 2 || nbEmeraude == 2 || nbDiamand == 2)
                    {
                        MessageBox.Show("Vous possédez déjà 2 jetons!");
                    }
                    else
                    {
                        totalCoins = nbRubis + nbOnyx + nbEmeraude + nbDiamand;

                        if (nbSaphir == 1 && totalCoins >= 1)
                        {
                            MessageBox.Show("Vous ne pouvez avoir qu'un jeton!");
                        }
                        else
                        {
                            if (nbSaphir + totalCoins != 3)
                            {
                                nbSaphir++;
                                availableSaphir--;
                                lblSaphirCoin.Text = availableSaphir.ToString();
                                lblChoiceSaphir.Text = nbSaphir + "\r\n";
                            }
                            else
                            {
                                MessageBox.Show("Vous avez atteint le nombre de jetons maximum!");
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// click on the black coin (onyx) to tell the player has selected this coin
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lblOnyxCoin_Click(object sender, EventArgs e)
        {
            if (enableClicLabel)
            {
                cmdValidateChoice.Visible = true;
                lblChoiceOnyx.Visible = true;

                if (availableOnyx == 2)
                {
                    MessageBox.Show("Ce type de jeton ne peut plus être retiré!");
                }
                else
                {
                    if (nbRubis == 2 || nbSaphir == 2 || nbOnyx == 2 || nbEmeraude == 2 || nbDiamand == 2)
                    {
                        MessageBox.Show("Vous possédez déjà 2 jetons!");
                    }
                    else
                    {
                        totalCoins = nbRubis + nbSaphir + nbEmeraude + nbDiamand;

                        if (nbOnyx == 1 && totalCoins >= 1)
                        {
                            MessageBox.Show("Vous ne pouvez avoir qu'un jeton!");
                        }
                        else
                        {
                            if (nbOnyx + totalCoins != 3)
                            {
                                nbOnyx++;
                                availableOnyx--;
                                lblOnyxCoin.Text = availableOnyx.ToString();
                                lblChoiceOnyx.Text = nbOnyx + "\r\n";
                            }
                            else
                            {
                                MessageBox.Show("Vous avez atteint le nombre de jetons maximum!");
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// click on the green coin (emeraude) to tell the player has selected this coin
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lblEmeraudeCoin_Click(object sender, EventArgs e)
        {
            if (enableClicLabel)
            {
                cmdValidateChoice.Visible = true;
                lblChoiceEmeraude.Visible = true;

                if (availableEmeraude == 2)
                {
                    MessageBox.Show("Ce type de jeton ne peut plus être retiré!");
                }
                else
                {
                    if (nbEmeraude == 2 || nbRubis == 2 || nbSaphir == 2 || nbOnyx == 2 || nbDiamand == 2)
                    {
                        MessageBox.Show("Vous possédez déjà 2 jetons d'un autre type!");
                    }
                    else
                    {
                        totalCoins = nbRubis + nbSaphir + nbOnyx + nbDiamand;

                        if (nbEmeraude == 1 && totalCoins >= 1)
                        {
                            MessageBox.Show("Vous ne pouvez avoir qu'un jeton!");
                        }
                        else
                        {
                            if (nbEmeraude + totalCoins != 3)
                            {
                                nbEmeraude++;
                                availableEmeraude--;
                                lblEmeraudeCoin.Text = availableEmeraude.ToString();
                                lblChoiceEmeraude.Text = nbEmeraude + "\r\n";
                            }
                            else
                            {
                                MessageBox.Show("Vous avez atteint le nombre de jetons maximum!");
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// click on the white coin (diamand) to tell the player has selected this coin
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lblDiamandCoin_Click(object sender, EventArgs e)
        {
            if (enableClicLabel)
            {
                cmdValidateChoice.Visible = true;
                lblChoiceDiamand.Visible = true;

                if (availableDiamand == 2)
                {
                    MessageBox.Show("Ce type de jeton ne peut plus être retiré!");
                }
                else
                {
                    if (nbDiamand == 2 || nbRubis == 2 || nbSaphir == 2 || nbOnyx == 2 || nbEmeraude == 2)
                    {
                        MessageBox.Show("Vous possédez déjà 2 jetons d'un autre type!");
                    }
                    else
                    {
                        totalCoins = nbRubis + nbSaphir + nbOnyx + nbEmeraude;

                        if (nbDiamand == 1 && totalCoins >= 1)
                        {
                            MessageBox.Show("Vous ne pouvez avoir qu'un jeton!");
                        }
                        else
                        {
                            if (nbDiamand + totalCoins != 3)
                            {
                                nbDiamand++;
                                availableDiamand--;
                                lblDiamandCoin.Text = availableDiamand.ToString();
                                lblChoiceDiamand.Text = nbDiamand + "\r\n";
                            }
                            else
                            {
                                MessageBox.Show("Vous avez atteint le nombre de jetons maximum!");
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// click on the validate button to approve the selection of coins or card
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmdValidateChoice_Click(object sender, EventArgs e)
        {
            totalCoins = nbRubis + nbSaphir + nbOnyx + nbEmeraude +nbDiamand;

            if (totalCoins > 1)
            {


                //coins reset
                nbDiamand = 0;
                nbOnyx = 0;
                nbRubis = 0;
                nbSaphir = 0;
                nbEmeraude = 0;

                cmdNextPlayer.Visible = true;
                cmdNextPlayer.Enabled = true;
            }          
        }

        /// <summary>
        /// click on the insert button to insert player in the game
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmdInsertPlayer_Click(object sender, EventArgs e)
        {
            MessageBox.Show("A implémenter");
        }

        /// <summary>
        /// click on the next player to tell him it is his turn
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmdNextPlayer_Click(object sender, EventArgs e)
        {
            if(id == 2)
            {
                id = 0;
            }
            else
            {
                id++;
            }
            //TO DO in release 1.0 : 3 is hard coded (number of players for the game), it shouldn't. 
            //TO DO Get the id of the player : in release 0.1 there are only 3 players
            //Reload the data of the player
            //We are not allowed to click on the next button
            
        }

        /// <summary>
        /// click on the player rubis coin to remove a coin
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lblChoiceRubis_Click(object sender, EventArgs e)
        {
            nbRubis--;
            availableRubis++;
            lblRubisCoin.Text = availableRubis.ToString();
            lblChoiceRubis.Text = nbRubis + "\r\n";

            if (nbRubis == 0)
            {
                lblChoiceRubis.Visible = false;
            }
        }

        /// <summary>
        /// click on the player saphir coin to remove a coin
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lblChoiceSaphir_Click(object sender, EventArgs e)
        {
            nbSaphir--;
            availableSaphir++;
            lblSaphirCoin.Text = availableSaphir.ToString();
            lblChoiceSaphir.Text = nbSaphir + "\r\n";

            if (nbSaphir == 0)
            {
                lblChoiceSaphir.Visible = false;
            }
        }

        /// <summary>
        /// click on the player onyx coin to remove a coin
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lblChoiceOnyx_Click(object sender, EventArgs e)
        {
            nbOnyx--;
            availableOnyx++;
            lblOnyxCoin.Text = availableOnyx.ToString();
            lblChoiceOnyx.Text = nbOnyx + "\r\n";

            if (nbOnyx == 0)
            {
                lblChoiceOnyx.Visible = false;
            }
        }

        /// <summary>
        /// click on the player emeraude coin to remove a coin
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lblChoiceEmeraude_Click(object sender, EventArgs e)
        {
            nbEmeraude--;
            availableEmeraude++;
            lblEmeraudeCoin.Text = availableEmeraude.ToString();
            lblChoiceEmeraude.Text = nbEmeraude + "\r\n";

            if (nbEmeraude == 0)
            {
                lblChoiceEmeraude.Visible = false;
            }
        }

        /// <summary>
        /// click on the player diamand coin to remove a coin
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lblChoiceDiamand_Click(object sender, EventArgs e)
        {
            nbDiamand--;
            availableDiamand++;
            lblDiamandCoin.Text = availableDiamand.ToString();
            lblChoiceDiamand.Text = nbDiamand + "\r\n";

            if (nbDiamand == 0)
            {
                lblChoiceDiamand.Visible = false;
            }
        }
    }
}
