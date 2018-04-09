<?php

//Variables for the connection
	$servername = "localhost";
	$server_username =  "u697839110_col";
	$server_password = "collin";
	$dbName = "u697839110_40k";
	
	//Make Connection
	$conn = new mysqli($servername, $server_username, $server_password, $dbName);
	//Check Connection
	if(!$conn){
		die("Connection Failed. ". mysqli_connect_error());
	}
	
	$sql = "SELECT * FROM `PHPtest`";
	$result = mysqli_query($conn, $sql);
	
	if(mysqli_num_rows($result) > 0)
	{
		
			while($row = mysqli_fetch_assoc($result))
		{
			echo $row['data'];
		}
		
	}
	


?>
