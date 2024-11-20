<?php
$servername = "localhost";
$username = "queen056_multiplayer";
$password = "@Shadow123@123";
$dbname = "queen056_shadowodyssey";

$name = $_POST['currentName'];

$moveForward = 'no';;
$moveBackward = 'no';;
$attack1 = 'no';
$attack2 = 'no';
$attack3 = 'no';
$isHit = 'no';
$currentHealth ='no'; 
$totalWins = 0;
$actualProfile = 0;
$isReady = 'no';
$autoFight = 'no';
$status = 'online';

$conn = new mysqli($servername, $username, $password, $dbname);

$stmt = $conn->prepare("INSERT INTO lobby(name, isMovingForward, isMovingBackward, isAttack1, isAttack2, isAttack3, isHit, currentHealth, totalWins, actualProfile, isReady, isAutoFight, status) VALUES (?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?)");

$stmt->bind_param("ssssssssiisss", $name, $moveForward, $moveBackward, $attack1, $attack2, $attack3, $isHit, $currentHealth, $totalWins, $actualProfile, $isReady, $autoFight, $status);

if ($stmt->execute()) {
    echo "success002";
} else {
    echo "error003";
}

$stmt->close();
$conn->close();