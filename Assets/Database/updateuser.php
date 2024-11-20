<?php
$servername = "localhost";
$username = "queen056_multiplayer";
$password = "@Shadow123@123";
$dbname = "queen056_shadowodyssey";

$collumn = $_POST['desiredCollumn'];
$value = $_POST['newValue'];
$validateRequest = $_POST['validateRequest'];

$conn = new mysqli($servername, $username, $password, $dbname);

// Escape the column name to prevent SQL injection
$escaped_column = $conn->real_escape_string($collumn);

$sql = "UPDATE lobby SET `$escaped_column` = ? WHERE id = ?";
$stmt = $conn->prepare($sql);
$stmt->bind_param("si", $value, $validateRequest);

if ($stmt->execute()) {
    echo "success004";
} else {
    echo "erro005";
}

$stmt->close();
$conn->close();
?>