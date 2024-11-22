<?php
$servername = "localhost";
$username = "queen056_multiplayer";
$password = "@Shadow123@123";
$dbname = "queen056_shadowodyssey";

$conn = new mysqli($servername, $username, $password, $dbname);

if ($conn->connect_error) {
    echo "erro001";
} else {
    $sql = "SELECT id, name, duel, host FROM lobby WHERE status = 'queue'";

    $result = $conn->query($sql);

    if ($result->num_rows > 0) {
        while ($row = $result->fetch_assoc()) {
            $combinedString = $row['id'] . " " . $row['name']  . " " . $row['duel']  . " " . $row['host'];
            echo $combinedString . "<br>"; // Add a newline character for better readability
        }
    } else {
        echo "error008";
    }
}

$conn->close();
?>