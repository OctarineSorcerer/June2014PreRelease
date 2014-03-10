'Skeleton Program code for the AQA COMP1 Summer 2014 examination
'this code should be used in conjunction with the Preliminary Material
'written by the AQA COMP1 Programmer Team
'developed in the Visual Studio 2008 (Console Mode) programming environment (VB.NET)

Module CardPredict

    Const NoOfRecentScores As Integer = 3

    Structure TCard
        Dim Suit As Integer 'the suit of the card, expressed as an integer
        Dim Rank As Integer 'the rank of the card, ie its number
        Dim TimesTurnedOver As Integer 'Track the amount of times this has been turned over
    End Structure

    Structure TRecentScore
        Dim Name As String 'name of the scoreholder
        Dim Score As Integer 'points of the scoreholder
    End Structure

    Sub Main()
        Dim Choice As Char 'container or the user's input
        Dim Deck(52) As TCard 'With this method, the array size does not have to increase
        Dim RecentScores(NoOfRecentScores) As TRecentScore 'Array of recent scores
        Randomize() 'initialises random-number generator
        Do
            DisplayMenu()
            Choice = GetMenuChoice() 'Choice becomes user input
            Select Case Choice 'does code based on value of Choice
                Case "1"
                    LoadDeck(Deck) 'calls the function to populate deck
                    ShuffleDeck(Deck) 'makes order of cards random
                    PlayGame(Deck, RecentScores) 'initialises game
                Case "2"
                    LoadDeck(Deck)
                    PlayGame(Deck, RecentScores)
                Case "3"
                    DisplayRecentScores(RecentScores) 'Displays recent scores
                Case "4"
                    ResetRecentScores(RecentScores) 'Resets recent scores
            End Select
        Loop Until Choice = "q" 'exits when choice is q
    End Sub

    Function GetRank(ByVal RankNo As Integer) As String 'Gets rank corresponding to integer
        Dim Rank As String = ""
        Select Case RankNo 'Sets output depending on RankNo
            Case 1 : Rank = "Ace"
            Case 2 : Rank = "Two"
            Case 3 : Rank = "Three"
            Case 4 : Rank = "Four"
            Case 5 : Rank = "Five"
            Case 6 : Rank = "Six"
            Case 7 : Rank = "Seven"
            Case 8 : Rank = "Eight"
            Case 9 : Rank = "Nine"
            Case 10 : Rank = "Ten"
            Case 11 : Rank = "Jack"
            Case 12 : Rank = "Queen"
            Case 13 : Rank = "King"
        End Select
        Return Rank
    End Function

    Function GetSuit(ByVal SuitNo As Integer) As String 'Gets correct suit for a number
        Dim Suit As String = ""
        Select Case SuitNo
            Case 1 : Suit = "Clubs"
            Case 2 : Suit = "Diamonds"
            Case 3 : Suit = "Hearts"
            Case 4 : Suit = "Spades"
        End Select
        Return Suit
    End Function

    Sub DisplayMenu()  'writes the menu onto console
        Console.WriteLine()
        Console.WriteLine("MAIN MENU")
        Console.WriteLine()
        Console.WriteLine("1.  Play game (with shuffle)")
        Console.WriteLine("2.  Play game (without shuffle)")
        Console.WriteLine("3.  Display recent scores")
        Console.WriteLine("4.  Reset recent scores")
        Console.WriteLine()
        Console.Write("Select an option from the menu (or enter q to quit): ")
    End Sub

    Function GetMenuChoice() As Char 'gets user input
        Dim Choice As Char
        Choice = Console.ReadLine
        Console.WriteLine()
        Return Choice
    End Function

    Sub LoadDeck(ByRef Deck() As TCard) 'populates a deck
        Dim Count As Integer
        FileOpen(1, "deck.txt", OpenMode.Input)
        Count = 1
        While Not EOF(1) 'while we haven't hit end of file
            Deck(Count).Suit = CInt(LineInput(1)) 'suit of correct card is set
            Deck(Count).Rank = CInt(LineInput(1)) 'rank of correct card is set
            Deck(Count).TimesTurnedOver = 0 'At initialisation, card has not been turned over
            Count = Count + 1
        End While
        FileClose(1)
    End Sub

    Sub ShuffleDeck(ByRef Deck() As TCard) 'shuffles the deck
        Dim NoOfSwaps As Integer 'times to swap
        Dim Position1 As Integer 'random position of first card
        Dim Position2 As Integer 'random position of second card
        Dim SwapSpace As TCard 'container for card at position1
        Dim NoOfSwapsMadeSoFar As Integer 'counter for the For loop
        NoOfSwaps = 1000 'no of times to loop
        For NoOfSwapsMadeSoFar = 1 To NoOfSwaps
            Position1 = Int(Rnd() * 52) + 1
            Position2 = Int(Rnd() * 52) + 1 'both of these are set to a random position in deck
            SwapSpace = Deck(Position1) 'gets card at position1
            Deck(Position1) = Deck(Position2) 'card at position2 is placed at position1
            Deck(Position2) = SwapSpace 'card at position2 is replaced with SwapSpace card
        Next
    End Sub

    Sub DisplayCard(ByVal ThisCard As TCard) 'writes out details of card onto console
        Console.WriteLine()
        Console.WriteLine("Card is the " & GetRank(ThisCard.Rank) & " of " & GetSuit(ThisCard.Suit))
        Console.WriteLine()
    End Sub

    Sub GetCard(ByRef ThisCard As TCard, ByRef Deck() As TCard, _
                ByVal NoOfCardsTurnedOver As Integer, ByVal DecksUsed As Integer)
        Dim Count As Integer
        ThisCard = Deck(1)
        Deck(1).TimesTurnedOver += 1
        If Deck(1).TimesTurnedOver = 2 Then 'If card has been used max amount of times
            For Count = 1 To (51 - NoOfCardsTurnedOver)
                Deck(Count) = Deck(Count + 1) 'brings cards left closer to the beginning of deck
            Next
            Deck(52 - NoOfCardsTurnedOver).Suit = 0
            Deck(52 - NoOfCardsTurnedOver).Rank = 0 'blanks the now-used card
        End If
    End Sub

    Function IsNextCardHigher(ByVal LastCard As TCard, ByVal NextCard As TCard) As Boolean
        Dim Higher As Boolean
        Higher = False
        If NextCard.Rank > LastCard.Rank Then
            Higher = True 'checks NextCard rank is higher than LastCard's
        End If
        Return Higher
    End Function

    Function GetPlayerName() As String 'gets player's name from player
        Dim PlayerName As String
        Console.WriteLine()
        Console.Write("Please enter your name: ")
        PlayerName = Console.ReadLine
        Console.WriteLine()
        Return PlayerName
    End Function

    Function GetChoiceFromUser() As Char
        Dim Choice As Char
        Console.Write("Do you think the next card will be higher than the last card (enter y or n)? ")
        Choice = Console.ReadLine
        Return Choice 'gets user's clairvoyant opinion
    End Function

    Sub DisplayEndOfGameMessage(ByVal Score As Integer, ByVal DecksUsed As Integer)
        Console.WriteLine()
        Console.WriteLine("GAME OVER!") 'game finished
        Console.WriteLine("Your score was " & Score)
        If Score = (52 * DecksUsed) - 1 Then 'For if they're special/sneaky enough to get max
            Console.WriteLine("WOW!  You completed a perfect game.")
        End If
        Console.WriteLine()
    End Sub

    Sub DisplayCorrectGuessMessage(ByVal Score As Integer) 'Person guessed correctly
        Console.WriteLine()
        Console.WriteLine("Well done!  You guessed correctly.")
        Console.WriteLine("Your score is now " & Score & ".")
        Console.WriteLine()
    End Sub

    Sub ResetRecentScores(ByRef RecentScores() As TRecentScore)
        Dim Count As Integer
        For Count = 1 To NoOfRecentScores 'loops through RecentScores and resets items
            RecentScores(Count).Name = ""
            RecentScores(Count).Score = 0
        Next
    End Sub

    Sub DisplayRecentScores(ByVal RecentScores() As TRecentScore)
        Dim Count As Integer
        Console.WriteLine()
        Console.WriteLine("Recent scores:")
        Console.WriteLine()
        For Count = 1 To NoOfRecentScores 'displays all recent scores
            Console.WriteLine(RecentScores(Count).Name & " got a score of " & RecentScores(Count).Score)
        Next
        Console.WriteLine()
        Console.WriteLine("Press the Enter key to return to the main menu")
        Console.WriteLine()
        Console.ReadLine()
    End Sub

    Sub UpdateRecentScores(ByRef RecentScores() As TRecentScore, ByVal Score As Integer)
        Dim PlayerName As String
        Dim Count As Integer
        Dim FoundSpace As Boolean
        PlayerName = GetPlayerName() 'gets name of player
        FoundSpace = False
        Count = 1
        While Not FoundSpace And Count <= NoOfRecentScores
            If RecentScores(Count).Name = "" Then 'if slot is empty
                FoundSpace = True
            Else
                Count = Count + 1
            End If
        End While
        If Not FoundSpace Then 'if no empty spaces
            For Count = 1 To NoOfRecentScores - 1
                RecentScores(Count) = RecentScores(Count + 1) 'essentially shifts them across
            Next
            Count = NoOfRecentScores
        End If
        RecentScores(Count).Name = PlayerName
        RecentScores(Count).Score = Score 'last entry is set
    End Sub

    Function GetNoOfDecksToUse()
        Console.WriteLine("How many decks do you want to use?")
        Dim userInput As Integer = Console.ReadLine()
        Do While userInput < 1
            Console.WriteLine("Input invalid, please enter a number > 0")
        Loop
        Return userInput
    End Function

    Sub PlayGame(ByVal Deck() As TCard, ByRef RecentScores() As TRecentScore)
        Dim NoOfCardsTurnedOver As Integer 'Number of cards player has turned over so far
        Dim DecksUsed As Integer = GetNoOfDecksToUse()
        Dim GameOver As Boolean 'Have they lost the game?
        Dim NextCard As TCard 'card about to be drawn
        Dim LastCard As TCard 'card last drawn
        Dim Higher As Boolean 'is the next card higher
        Dim Choice As Char 'user input
        GameOver = False 'They have not lost the game yet
        GetCard(LastCard, Deck, 0, DecksUsed) 'gets next card
        DisplayCard(LastCard) 'displays the card
        NoOfCardsTurnedOver = 1 'increments cards turned over
        While NoOfCardsTurnedOver < 52 * DecksUsed And Not GameOver 'while game hasn't ended
            GetCard(NextCard, Deck, NoOfCardsTurnedOver, DecksUsed) 'get next card
            Do
                Choice = GetChoiceFromUser()
            Loop Until Choice = "y" Or Choice = "n" 'loop until valid user input
            DisplayCard(NextCard) 'display the next card
            NoOfCardsTurnedOver = NoOfCardsTurnedOver + 1 'increment no of cards turned over
            Higher = True 'IsNextCardHigher(LastCard, NextCard) 'checks if this card is higher than last
            If Higher And Choice = "y" Or Not Higher And Choice = "n" Then 'If their choice is correct
                DisplayCorrectGuessMessage(NoOfCardsTurnedOver - 1) 'display correct message
                LastCard = NextCard 'sets lastcard to card just drawn
            Else
                GameOver = True 'they guessed wrong
            End If
        End While
        If GameOver Then 'they died
            DisplayEndOfGameMessage(NoOfCardsTurnedOver - 2, DecksUsed) 'display their score
            UpdateRecentScores(RecentScores, NoOfCardsTurnedOver - 2) 'update recent scores
        Else 'they got through all the cards
            DisplayEndOfGameMessage((52 * DecksUsed) - 1, DecksUsed)
            UpdateRecentScores(RecentScores, 51) 'update recent scores with perfect score
        End If
    End Sub
End Module
