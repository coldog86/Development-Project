
<?php
//Variables for the connection
	$servername = "localhost";
	$server_username =  "u697839110_col";
	$server_password = "collin";
	$dbName = "u697839110_40k";
	
	$data = $_POST["dataPost"];
	
	
	//Make Connection
	$conn = new mysqli($servername, $server_username, $server_password, $dbName);
	//Check Connection
	if(!$conn){
		die("Connection Failed. ". mysqli_connect_error());
	}
	//else echo ("Connection Success <br>" );
	
	$sql = "UPDATE PHPtest
SET data = '$data' ;";
	if(!(mysqli_query($conn ,$sql))) echo mysqli_error($conn);
    else echo "Everything ok."
?>

