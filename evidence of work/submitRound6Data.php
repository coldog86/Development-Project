
<?php

//Variables for the connection
	$servername = "localhost";
	$server_username =  "u697839110_col";
	$server_password = "collin";
	$dbName = "u697839110_40k";
	
	 $gameNumber = $_POST["gameNumberPost"];
	 $opponentScore = $_POST["opponentScorePost"];
	 $turnNumber = $_POST["turnsCompletedPost"];
	 
	//Make Connection
	$conn = new mysqli($servername, $server_username, $server_password, $dbName);
	//Check Connection
	if(!$conn){
		die("Connection Failed. ". mysqli_connect_error());
	}
	
    
                            
//Make Connection
	$conn = new mysqli($servername, $server_username, $server_password, $dbName);
	//Check Connection
	if(!$conn){
		die("Connection Failed. ". mysqli_connect_error());
	}
	//else echo ("Connection Success <br>" );
	
	$sql = "DELETE from OngoingGames 
            WHERE gameNumber=$gameNumber
            ";
    
 if(!(mysqli_query($conn ,$sql))) echo mysqli_error($conn);
    else echo "Everything ok."
	


?>
