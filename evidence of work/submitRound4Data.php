
<?php

//Variables for the connection
	$servername = "localhost";
	$server_username =  "u697839110_col";
	$server_password = "collin";
	$dbName = "u697839110_40k";
	
	 $gameNumber = $_POST["gameNumberPost"];
	 $score = $_POST["scorePost"];
	 $turnNumber = $_POST["turnsCompletedPost"];
	 $overAllScore = $_POST["overAllScorePost"];
	 
	//Make Connection
	$conn = new mysqli($servername, $server_username, $server_password, $dbName);
	//Check Connection
	if(!$conn){
		die("Connection Failed. ". mysqli_connect_error());
	}
	
    $sql = "UPDATE OngoingGames 
            SET Score=0, turnNumber=?, overAllScore=?
            WHERE gameNumber=$gameNumber
            ";
                            
    if($stmt = mysqli_prepare($conn, $sql)){
    
        mysqli_stmt_bind_param($stmt, "ii", $turnNumber, $overAllScore);
    
        
        $stmt->execute();
        
        echo "Ongoing game updated successfully.";
        
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
