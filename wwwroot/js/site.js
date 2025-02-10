// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
$(document).ready(() => {
  $(".create-post-toggle").click((e) => {
    e.preventDefault();
    $(".create-post").toggleClass("display-none");
  });
});
