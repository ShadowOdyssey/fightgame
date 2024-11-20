<?php
$servername = "localhost";
$username = "queen056_multiplayer";
$password = "@Shadow123@123";
$dbname = "queen056_shadowodyssey";

$value = $_POST['newValue'];
$collumn = $_POST['desiredCollumn'];
$validateCollumn = $_POST['validateCollumn'];
$searchCollumn = $_POST['searchCollumn'];

$conn = new mysqli($servername, $username, $password, $dbname);

$sql = "UPDATE lobby SET ? = ? WHERE ? = ?";
$stmt = $conn->prepare($sql);
$stmt->bind_param("ss", $collumn, $validateCollumn, $seachCollumn, $value);

if ($stmt->execute()) {
    echo "success004";
} else {
    echo "erro005";
}

$stmt->close();

$conn->close();
?>