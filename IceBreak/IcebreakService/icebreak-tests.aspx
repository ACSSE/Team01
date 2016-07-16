<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
    <head>
        <title>IceBreak</title>
        <script src="http://ajax.googleapis.com/ajax/libs/jquery/1.10.2/jquery.min.js"></script>
        <script>
            function registerUser()
            {
                var personNameVar = "Paul";
                var dataIn = '{' + '"personName":"' + personNameVar + '"}';
                $.ajax({
                    url: "/IBUserRequestService.svc/usrRegPOST",
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    data: dataIn,
                    dataType: "json",
                    success: function (data)
                    {
                        alert("Success: "+data);
                    },
                    error: function (error)
                    {
                        alert("Error: " + error.Error);
                    }
                });
             }
        </script>
    </head>
    <body>
        <button onclick="registerUser()">Register</button>
    </body>
</html>

