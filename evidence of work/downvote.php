	
<?php

//Variables for the connection
	$servername = "localhost";
	$server_username =  "u697839110_col";
	$server_password = "collin";
	$dbName = "u697839110_40k";
	
	 $questionText = $_POST["questionPost"];
	 
	 
	//Make Connection
	$conn = new mysqli($servername, $server_username, $server_password, $dbName);
	//Check Connection
	if(!$conn){
		die("Connection Failed. ". mysqli_connect_error());
	}
	
    $sql = "UPDATE q2 
set Upvotes = Upvotes-1 
WHERE QuestionText = ?";
                            
    if($stmt = mysqli_prepare($conn, $sql)){
    
        mysqli_stmt_bind_param($stmt, "s", $questionText);
    
        
        $stmt->execute();
        
        echo "Records inserted successfully.";
        
    } 

	


?>
