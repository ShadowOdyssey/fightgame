<?php
$servername = "localhost";
$username = "queen056_multiplayer";
$password = "@Shadow1234@1234";
$dbname = "queen056_shadowodyssey";

$name = $_POST['currentName'];
$status = $_POST['newStatus'];

$conn = new mysqli($servername, $username, $password, $dbname);

$sql = "UPDATE lobby SET status = ? WHERE name = ?";
$stmt = $conn->prepare($sql);
$stmt->bind_param("ss", $status, $name);

if ($stmt->execute()) {
    echo "success001";
} else {
    echo "error001";
}

$stmt->close();
$conn->close();
?>