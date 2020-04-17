// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
$(function () {
    if ($("#register_popup") !== undefined) {
        $('#register_popup').modal('show');
    }    if ($("#login_popup") !== undefined) {
        $('#login_popup').modal('show');
    }
    // re-pop modal to show newly created add message
    if (typeof $("#selectedId").val() !== "undefined" && $("#selectedId").val() !== "") {
        let data = $("#catbtn" + $("#selectedId").val()).data("details");
        CopyToModal($("#selectedId").val(), data);
        $("#details_popup").modal("show");
    }
    // details click - to load popup on catalogue
    $("a.btn-outline-dark").on("click", (e) => {
        $("#results").text("");
        let id = e.target.dataset.id;
        let data = JSON.parse(e.target.dataset.details); // it's a string need an object
        CopyToModal(id, data);
    });
    $(".nav-tabs a").on("show.bs.tab", function (e) {
        if ($(e.relatedTarget).text() === "Demographic") { // tab 1
            $("#Firstname").valid();
            $("#Lastname").valid();
            $("#Age").valid();
            $("#CreditCardType").valid();
            if ($("#Firstname").valid() === false || $("#Lastname").valid() === false || $("#Age").valid() === false || $("CreditCardType").vaid() === false) {
                return false; // suppress click
            }
        }
        if ($(e.relatedTarget).text() === "Address") { // tab 2
            $("#Address1").valid();
            $("#City").valid();
            $("#Country").valid();
            $("#Region").valid();
            $("#Mailcode").valid();
            if ($("#Address1").valid() === false || $("#City").valid() === false || $("#Country").valid() === false || $("#Region").valid() === false || $("#Mailcode").valid() === false) {
                return false; // suppress click
            }
        }
        if ($(e.relatedTarget).text() === "Account") { // tab 3
            $("#Email").valid();
            $("#Password").valid();
            $("#RepeatPassword").valid();
            if ($("#Email").valid() === false || $("#Password").valid() === false ||
                $("#RepeatPassword").valid() === false) {
                return false; // suppress click
            }
        }
    }); // show bootstrap tab
});

// populate the modal fields
const CopyToModal = (id, data) => {
    $("#qty").val("0");
    $("#productname").text(data.PRODUCTNAME);
    $("#graphicname").text(data.GRAPHICNAME);
    $("#costprice").text(data.COSTPRICE);
    $("#msrp").text(data.MSRP);
    $("#qtyonhand").text(data.QTYONHAND);
    $("#qtyonbackorder").text(data.QTYONBACKORDER);
    $("#description").text(data.Description);
    $("#detailsGraphic").attr({ "src": `/images/${data.GRAPHICNAME}`, "width": "150px" });
    $("#selectedId").val(id);
};