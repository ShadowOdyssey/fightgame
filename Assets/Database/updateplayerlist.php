<?php
$servername = "localhost";
$username = "queen056_multiplayer";
$password = "@Shadow123@123";
$dbname = "queen056_shadowodyssey";

$conn = new mysqli($servername, $username, $password, $dbname);

if ($conn->connect_error) {
    echo "erro001";
} else {
    $sql = "SELECT id, name, profile, wins, ready, status FROM lobby WHERE status = 'online'";

    $result = $conn->query($sql);

    if ($result->num_rows > 0) {
        while ($row = $result->fetch_assoc()) {
            $combinedString = $row['id'] . " " . $row['name'] . " " . $row['profile'] . " " . $row['wins'] . " " . $row['ready'] . " " . $row['status'];
            echo $combinedString . "<br>"; // Add a newline character for better readability
        }
    } else {
        echo "error006";
    }
}

$conn->close();
?>