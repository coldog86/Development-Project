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
	
	$sql = "SELECT * FROM `Ranking`";
	$result = mysqli_query($conn, $sql);
	
	if(mysqli_num_rows($result) > 0)
	{
		$array = array();
			while($row = mysqli_fetch_assoc($result))
		{
			//$array[] = array('totalScore' => $row["totalScore"]);
		
			echo $row["score"]. "<br>";
		}
		
	}
	

	
    //echo '<pre>', print_r($array, true), '</pre>';
    $string = json_encode($array);
	//echo $string;
?>
