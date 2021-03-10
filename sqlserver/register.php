<?php

	$con = mysqli_connect('localhost', 'root', '', 'unityaccess');

	//check if the connection happens
	if (mysqli_connect_errno()) 
	{
		echo "1: Connection failed"; // error code #1 meaning connection failed
		exit();
	}

	$username = $_POST["name"];
	$password = $_POST["password"];

	//checks if username exists in database

	$namecheckquery = "SELECT Username FROM players WHERE Username='"  . $username . "';";

	$namecheck = mysqli_query($con, $namecheckquery) or die("2: Name check query failed"); //error code #2 meaning namecheck query failed

	if (mysqli_num_rows($namecheck) > 0) 
	{
		echo "3: Name already exists in database"; //error code #3 - name exists cannot register
		exit();
	}

	//add user to the table
	$salt = "\$5\$rounds=5000\$" . "steamhams" . $username . "\$";
	$hash = crypt($password, $salt);
	$insertuserquery = "INSERT INTO players (Username, Hash, Salt) VALUES ('" . $username . "', '" . $hash . "', '" . $salt . "');";
	mysqli_query($con, $insertuserquery) or die("4: Insert player query failed"); //error code #4 - insert query failed

	echo ("0"); //success code

?>
