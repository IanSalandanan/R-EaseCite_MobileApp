<?php 

include_once('connects_recit.php');

class CRUD{

    private $con;

    public function __construct($con) {
        $this->con = $con;
    }
    
    public function display($query, $columnName, $whereParam = null)
    {
        try {
            $stmt = $this->con->prepare($query);

            if ($stmt === false) {
                throw new Exception(json_encode(array("error message" => "Preparation error: " . $this->con->error)));
            }

            if (!is_null($whereParam)) {
                $stmt->bind_param("s", $whereParam);
            }

            if (!$stmt->execute()) {
                throw new Exception(json_encode(array("error message" => "Execution error: " . $stmt->error)));
            }

            $result = $stmt->get_result();

            if ($result === false) {
                throw new Exception(json_encode(array("error message" => "Getting result failed: " . $stmt->error)));
            }

            $questions = array();

            while ($row = $result->fetch_assoc()) {
                $questions[] = $row[$columnName];
            }

            if (empty($questions)) {
                echo json_encode(array("message" => "No data found."));
            } else {
                echo json_encode($questions);
            }

            $result->free();
            $stmt->close();

        } catch (Exception $e) {
            echo json_encode(array("error message" => "Display Error: " . $e->getMessage()));
        }
    }

       
    function add($query, $insert, $insert1 = null) 
    {
        try {
            if (empty($insert)) {
                throw new Exception("Invalid input.");
            }

            if (!is_null($insert1))
            {               
                $paramTypes = "ss";
                $stmt = $this->executeQuery($query, $paramTypes, $insert, $insert1);

            }
            else {
                
                $paramTypes = "s";
                $stmt = $this->executeQuery($query, $paramTypes, $insert);

            }
          
            if ($stmt == true) {
                echo "'$insert' added.";
            } 
            else 
            {
                throw new Exception("Execution error: " . $stmt->error);
            }

        } catch (Exception $e) {

            if (str_contains($e->getMessage(), 'Duplicate entry')) {echo "'$insert' already added.";}
            else {echo "Error: " . $e->getMessage();}
        }
    }
       
    function remove($query, $delete, $delete1 = null)
    {   
        try {

            if (empty($delete)) {
                throw new Exception("Invalid input.");
            }
    
            if (!is_null($delete1))
            {               
                $paramTypes = "ss";
                $affected_rows = $this->executeQuery($query, $paramTypes, $delete, $delete1);         
            }
            else {
                
                $paramTypes = "s";
                $affected_rows = $this->executeQuery($query, $paramTypes, $delete);               
            }
          
            if ($affected_rows == 0) {
                echo "'$delete' does not exist.";
            } 
            else if ($affected_rows > 0) {
                echo "'$delete' removed.";
            } 
            else {
                throw new Exception("Execution error:" . $affected_rows->error);
            }

        } catch (Exception $e) {
           
            echo "Error: " . $e->getMessage();
        }
    }

    function update($query, $update, $origData, $suppData = null)
    {
        try {
            if (empty($update)) {
                throw new Exception("Invalid input.");
            }
    
            if (!is_null($suppData))
            {               
                $paramTypes = "sss";
                $affected_rows = $this->executeQuery($query, $paramTypes, $update, $origData, $suppData);         
            }
            else {
                
                $paramTypes = "ss";
                $affected_rows = $this->executeQuery($query, $paramTypes, $update, $origData);               
            }
          
            if ($affected_rows == 0) {
                echo "No changes.";
            } 
            else if ($affected_rows > 0) {
                echo "'$update' updated.";
            } 
            else {
                throw new Exception("Execution error:" . $affected_rows->error);
            }
    
        } catch (Exception $e) {
            echo "Error: " . $e->getMessage();
        }
    }

    private function executeQuery($query, $paramTypes = null, ...$params) 
    {
        $stmt = $this->con->prepare($query);

        if ($stmt === false) {
            throw new Exception("Preparation error: " . $this->con->error);
        }

        if (!empty($paramTypes) && !empty($params)) {
            $stmt->bind_param($paramTypes, ...$params);
        }

        if (!$stmt->execute()) {
            $stmt->close();
            throw new Exception("Execution error: " . $stmt->error);
        } 
        else 
        { 
            $affected_rows = $stmt->affected_rows; 
            $stmt->close();

            return $affected_rows;
        }       
    }
}
?>
