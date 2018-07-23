
<?php

//Variables for the connection
	$servername = "localhost";
	$server_username =  "u697839110_col";
	$server_password = "collin";
	$dbName = "u697839110_40k";
	
	 $gameNumber = $_POST["gameNumberPost"];
	 $player = $_POST["playerNamePost"];
	 $askedQuestions = $_POST["askedQuestionsPost"];
	 $QuestionsLeftInCatagory = $_POST["QuestionsLeftInCatagoryPost"];
	 $Round1Catagory = $_POST["Round1CatagoryPost"];
	 $score = $_POST["scorePost"];
	 $turnNumber = $_POST["turnsCompletedPost"];
	 
	 $userID = $_POST["userIDPost"];
	 $totalGamesPlayed = $_POST["totalGamesPlayedPost"];
	 $totalQuestions = $_POST["totalQuestionsPost"];
	 $totalCorrectQuestions = $_POST[""];
	 
	//Make Connection
	$conn = new mysqli($servername, $server_username, $server_password, $dbName);
	//Check Connection
	if(!$conn){
		die("Connection Failed. ". mysqli_connect_error());
	}
	
   $sql = "INSERT INTO OngoingGames 
                            (gameNumber, player, askedQuestions, QuestionsLeftInCatagory, Round1Catagory, Score, turnNumber) 
                            VALUES (?, ?, ?, ?, ?, ?, ?);
            
        ";                 
                            
    if($stmt = mysqli_prepare($conn, $sql)){
    
        mysqli_stmt_bind_param($stmt, "issssii", $gameNumber, $player, $askedQuestions, $QuestionsLeftInCatagory, $Round1Catagory, $score, $turnNumber);
    
        
        $stmt->execute();
        
        echo "Ongoing game updated successfully.";
        
    } 
	
	if(!$conn){
		die("Connection Failed. ". mysqli_connect_error());
	}
	
   $sql = "
            UPDATE Users 
        SET numGamesPlayed=$totalGamesPlayed, totalPointsScore=totalPointsScore + $score, TotalCorrectAnswers=$totalQuestions, TotalCorrectAnswers=$totalCorrectQuestions
        WHERE ID=$userID;
        ";                 
                            
    if($stmt = mysqli_prepare($conn, $sql)){
    
        mysqli_stmt_bind_param($stmt, "issssii", $gameNumber, $player, $askedQuestions, $QuestionsLeftInCatagory, $Round1Catagory, $score, $turnNumber);
    
        
        $stmt->execute();
        
        echo "Ongoing game updated successfully.";
        
    } 
	


?>
