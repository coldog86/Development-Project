
<?php

//Variables for the connection
	$servername = "localhost";
	$server_username =  "u697839110_col";
	$server_password = "collin";
	$dbName = "u697839110_40k";
	
	 $username = $_POST["usernamePost"];
	 $score = $_POST["scorePost"];
	 
	//Make Connection
	$conn = new mysqli($servername, $server_username, $server_password, $dbName);
	//Check Connection
	if(!$conn){
		die("Connection Failed. ". mysqli_connect_error());
	}
	
    $sql = "INSERT INTO HighScores 
                            (userName, totalScore) 
                            VALUES (?, ?)";
                            
    if($stmt = mysqli_prepare($conn, $sql)){
    
        mysqli_stmt_bind_param($stmt, "ss", $username, $score);
    
        
        $stmt->execute();
        
        echo "Records inserted successfully.";
        
    } 
	
	$query = mysqli_query($conn, $sql);
	
    $rows = array();
    if($row = mysqli_fetch_assoc($query)) {
        $rows[] = $row;
    }
    print json_encode($row);
	
	
	$return_arr = array();
		
	
	


?>
