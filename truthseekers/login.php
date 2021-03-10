<?php

	$con = mysqli_connect('localhost', 'root', '', 'unityaccess');

	//check if the connection happens
	if (mysqli_connect_errno()) 
	{
		echo "1: Connection failed"; // error code #1 meaning connection failed
		exit();
	}

	$username = mysqli_real_escape_string($con, $_POST["name"]);
	$usernameclean = filter_var($username, FILTER_SANITIZE_STRING, FILTER_FLAG_STRIP_LOW | FILTER_FLAG_STRIP_HIGH);

	$password = $_POST["password"];

	//checks if username exists in database

	$namecheckquery = "SELECT Username, Salt, Hash, Score FROM players WHERE Username='"  . $usernameclean . "';";

	$namecheck = mysqli_query($con, $namecheckquery) or die("2: Name check query failed"); //error code #2 meaning namecheck query failed

	if (mysqli_num_rows($namecheck) != 1)
	{
		echo "5: Either no user with name, or more than one"; //error code $5 - number of names matching != 1
		exit();
	}
	//get login info from query
	$existinginfo = mysqli_fetch_assoc($namecheck);

	$salt = $existinginfo["Salt"];
	$hash = $existinginfo["Hash"];

	$loginhash = crypt($password, $salt);
	if($hash != $loginhash)
	{
		echo "6: Incorrect Password"; //error code #6 - password does not hash to match table
		exit();
	}
	
	echo "0\t" . $existinginfo["Score"];


?>