<?php
    $db = new PDO('sqlite:./clipboard.db');
    $db->setAttribute(PDO::ATTR_DEFAULT_FETCH_MODE, PDO::FETCH_ASSOC);
    $db->setAttribute(PDO::ATTR_ERRMODE, PDO::ERRMODE_EXCEPTION);
    $stmt = $db->prepare('SELECT hostname, timedate, data FROM clipboard order by id desc LIMIT 50');
    $stmt->execute(); 

    while($req = $stmt->fetch()) {
        echo '<tr>
            <td>'.$req['hostname'].'</td>
            <td>'.$req['timedate'].'</td>
            <td>'.$req['data'].'</td>
        </tr>';
    }
?>