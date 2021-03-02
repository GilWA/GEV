$(function () {
    "use strict";

    $("#btnRechercher").click(function () {
        if ($("#txtCpVille").val() === "") {
            $("#divResultats").html("Entrer un code postal ou une ville");
            return;
        }
        $.ajax({
            url: "/api/adresses/rad/" + $("#txtCpVille").val(),
            type: "GET",
            dataType: "json",
            contentType: 'application/x-www-form-urlencoded',
            success:
                function (data) {
                    console.log("data:" + data);

                    var resultats = "";
                    if (data.length === 0) {
                        resultats = "<p>Aucune adresse n'a été trouvée</p>";
                    } else {
                        for (var i = 0; i < data.length; i++) {
                            var adresse = data[i].ville;
                            if (data[i].codePostal === "") adresse += " - " + data[i].codePostal;
                            resultats += "<p><input type='button' value='Choisir cette adresse' class='btnConfirm' " +
                                         "data-toggle='modal' data-target='#divConfirm' " +
                                         "data-adresse='" + adresse + "' " +
                                         "onclick='confirmation(" + data[i].adresseId + ", this);'/>" + adresse + "</p>";
                        }
                    }
                    console.log("resultat:" + resultats);
                    $("#divResultats").html(resultats);
                },
            error: function (xhr, status, error) {
                $("#divResultats").html("Erreur  : " + status + " " + error);
            }
        });

    });

    $("#btnCreer").click(creation);
});

function confirmation(ida, btn) {
    $("#ida").val(ida);
    //console.log($(btn).attr("data-adresse"));
    var adresse = $(btn).attr("data-adresse");
    $("#libAd").text("Etes-vous sur de vouloir associer l'adresse " + adresse + " à cet événement ?");
}

function creation() {
    //alert($("#idc").text() + "," + $("#ida").text());
    var ide = $("#ide").val();
    var ida = $("#ida").val();

    $.ajax({
        url: "/api/evenements/ca/" + ide + "/" + ida,
        type: "POST",
        dataType: "text",
        contentType: 'application/x-www-form-urlencoded',
        success:
            function (data) {
                window.location.replace("/evenements/details/" + ide);
            },
        error: function (xhr, status, error) {
            alert("Erreur  : " + status + " " + error);
        }
    });
}