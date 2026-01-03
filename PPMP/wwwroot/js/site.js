// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
function TogglePassword() {
  var x = document.getElementById("passwordToggle");
  if (x.type === "password") {
    x.type = "text";
  } else {
    x.type = "password";
  }
}