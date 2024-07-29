<?php

include_once('crud_functions.php'); 

$tableN = $_GET['table'];
$where = isset($_GET['param']) ? $_GET['param'] : null;

$crud = new CRUD($con);

try { 
    if (empty($tableN)) {
        throw new Exception("Invalid input.");
    }

    if ($tableN === 'subject' && is_null($where))
    {
        $colName = 'subj_id';
        $query = "SELECT subj_id FROM subject";

        $crud->display($query, $colName);
              
    } 
    else if ($tableN === 'question')
    {
        $colName = 'ques_name';
        $query = "SELECT $colName FROM question WHERE subj_id = ?";

        $crud->display($query, $colName, $where);
    }
    else if ($tableN === 'section' && is_null($where))
    {
        $colName = 'sect_id';
        $query = "SELECT $colName FROM section";

        $crud->display($query, $colName);
    }
    else if ($tableN === 'student')
    {
        $colName = 'stud_name';
        $query = "SELECT $colName FROM student WHERE sect_id = ?";

        $crud->display($query, $colName, $where);
    }

} catch (Exception $e) {
    echo "Error: " . $e->getMessage();
}

$con->close();

?>