﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using TrackerLibrary;
using TrackerLibrary.Models;

namespace TrackerUI1
{
    public partial class TournamentViewerForm : Form
    {
        private TournamentModel tournament;
        BindingList<int> rounds = new BindingList<int>();
        BindingList<MatchupModel> selectedMatchups = new BindingList<MatchupModel>();

        public TournamentViewerForm(TournamentModel tournamentModel)
        {
            InitializeComponent();

            tournament = tournamentModel;

            tournament.OnTournamentComplete += Tournament_OnTournamentComplete; 

            WireUpLists();

            LoadFormData();

            LoadRounds();
        }

        private void Tournament_OnTournamentComplete(object sender, DateTime e)
        {
            this.Close();
        }

        //private void Tournament_OnTournamentComplete(object sender, DateTime e)
        //{
        //    this.Close();
        //}
        private void LoadFormData()
        {
            tournamentName.Text = tournament.TournamentName;
        }


        private void WireUpLists()
        {
            roundDropDown.DataSource = rounds;
            matchupListBox.DataSource = selectedMatchups;
            matchupListBox.DisplayMember = "DisplayName";
        }


        private void LoadRounds()
        {
            rounds.Clear();

            rounds.Add(1);
            int currentRound = 1;

            foreach (List<MatchupModel> matchups in tournament.Rounds)
            {
                if (matchups.First().MatchupRound > currentRound)
                {
                    currentRound = matchups.First().MatchupRound;
                    rounds.Add(currentRound);
                }
            }

            LoadMatchups(1);
        }


        //MATCHUPS PLURAL

        private void LoadMatchups(int round)
        {
            foreach (List<MatchupModel> matchups in tournament.Rounds)
            {
                if (matchups.First().MatchupRound == round)
                {
                    //It looks like this line is clearing out all the correct matchup information after scoring, effectively resetting it to round 1 info prior to scoring. 
                    selectedMatchups.Clear();
                    foreach (MatchupModel m in matchups)
                    {
                        if (m.Winner == null || !unplayedOnlyCheckbox.Checked)
                        {
                            selectedMatchups.Add(m);
                        }
                    }
                }
            }

            if (selectedMatchups.Count > 0)
            {
                LoadMatchup(selectedMatchups.First());
            }

            DisplayMatchupInfo();
        }

        private void DisplayMatchupInfo()
        {
            //Every time we load matchups, run this check. If we won't have values in our matchups, nothing should be visible. 
            bool isVisible = (selectedMatchups.Count > 0);

            teamOneName.Visible = isVisible;
            teamOneScoreLabel.Visible = isVisible;
            teamOneScoreValue.Visible = isVisible;

            teamTwoName.Visible = isVisible;
            teamTwoScoreLabel.Visible = isVisible;
            teamTwoScoreValue.Visible = isVisible;

            versusLabel.Visible = isVisible;

            scoreButton.Visible = isVisible;
        }


        //MATCHUP SINGULAR
        private void LoadMatchup(MatchupModel m)
        {
            if (m != null)
            {
                for (int i = 0; i < m.Entries.Count; i++)
                {
                    if (i == 0)
                    {
                        if (m.Entries[0].TeamCompeting != null)
                        {
                            teamOneName.Text = m.Entries[0].TeamCompeting.TeamName;
                            teamOneScoreValue.Text = m.Entries[0].Score.ToString();

                            teamTwoName.Text = "<bye>";
                            teamTwoScoreValue.Text = "0";
                        }
                        else
                        {
                            teamOneName.Text = "Not Yet Set";
                            teamOneScoreValue.Text = "";
                        }
                    }

                    if (i == 1)
                    {
                        if (m.Entries[1].TeamCompeting != null)
                        {
                            teamTwoName.Text = m.Entries[1].TeamCompeting.TeamName;
                            teamTwoScoreValue.Text = m.Entries[1].Score.ToString();
                        }
                        else
                        {
                            teamTwoName.Text = "Not Yet Set";
                            teamTwoScoreValue.Text = "";
                        }
                    }
                }
            }
        }

        private string ValidateData()
        {
            string output = "";
            double teamOneScore = 0;
            double teamTwoScore = 0;

            bool scoreOneValid = double.TryParse(teamOneScoreValue.Text, out teamOneScore);
            bool scoreTwoValid = double.TryParse(teamTwoScoreValue.Text, out teamTwoScore);

            if (!scoreOneValid)
            {
                output = "The score one value is not a valid number";
            }
            else if (!scoreTwoValid)
            {
                output = "The score two value is not a valid number";
            }
            else if (teamOneScore == 0 && teamTwoScore == 0)
            {
                output = "You did not enter a score for either team";
            }
            else if (teamOneScore == teamTwoScore)
            {
                output = "We do not allow ties in this application";
            }

            return output;
        }

        private void scoreButton_Click(object sender, EventArgs e)
        {

            string errorMessage = ValidateData();

            if (errorMessage.Length > 0)
            {
                MessageBox.Show($"Input Error: { errorMessage }");
                return;
            }

            MatchupModel m = (MatchupModel)matchupListBox.SelectedItem;

            double teamOneScore = 0;
            double teamTwoScore = 0;

            if (m != null)
            {
                for (int i = 0; i < m.Entries.Count; i++)
                {
                    if (i == 0)
                    {
                        if (m.Entries[0].TeamCompeting != null)
                        {
                            m.Entries[0].TeamCompeting.TeamName = teamOneName.Text;

                            bool scoreValid = double.TryParse(teamOneScoreValue.Text, out teamOneScore);

                            if (scoreValid)
                            {
                                m.Entries[0].Score = teamOneScore;
                            }
                            else
                            {
                                MessageBox.Show("Please enter a valid score for team 1.");
                                return;
                            }

                        }
                        else
                        {
                            teamOneName.Text = "Not Yet Set";
                            teamOneScoreValue.Text = "";
                        }
                    }

                    if (i == 1)
                    {
                        if (m.Entries[1].TeamCompeting != null)
                        {
                            bool scoreValid = double.TryParse(teamTwoScoreValue.Text, out teamTwoScore);
                            if (scoreValid)
                            {
                                m.Entries[1].Score = teamTwoScore;
                            }
                            else
                            {
                                MessageBox.Show("Please enter a valid score for team 2.");
                                return;
                            }
                        }
                    }
                }
                try
                {
                    TournamentLogic.UpdateTournamentResults(tournament);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"The application had the following error: { ex.Message } ");
                    return;
                }

                LoadMatchups((int)roundDropDown.SelectedItem);
            }
        }

        private void matchupListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadMatchup((MatchupModel)matchupListBox.SelectedItem);
        }

        private void unplayedOnlyCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            LoadMatchups((int)roundDropDown.SelectedItem);
        }

        private void roundDropDown_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadMatchups((int)roundDropDown.SelectedItem);
        }
    }
}

           

