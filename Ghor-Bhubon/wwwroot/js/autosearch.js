//$(document).ready(function () {
//    // Listen for keyup events on the area input field
//    $('#Area').on('keyup', function () {
//        var area = $(this).val();
//        var city = $('#City').val();

//        if (area && city) {
//            // Make an AJAX request to fetch matching areas
//            $.ajax({
//                url: '@Url.Action("GetMatchingAreas", "Landlord")',
//                type: 'GET',
//                data: { city: city, area: area },
//                success: function (data) {
//                    // Show matching areas in a dropdown below the input
//                    var suggestions = $('#areaSuggestions');
//                    suggestions.empty(); // Clear existing suggestions

//                    if (data.length > 0) {
//                        data.forEach(function (item) {
//                            suggestions.append('<li class="suggestion-item">' + item + '</li>');
//                        });
//                    } else {
//                        suggestions.append('<li>No matching areas found</li>');
//                    }

//                    // Show suggestion list
//                    suggestions.show();
//                }
//            });
//        }
//    });

//    // When a suggestion is clicked, populate the Area input
//    $(document).on('click', '.suggestion-item', function () {
//        $('#Area').val($(this).text());
//        $('#areaSuggestions').hide(); // Hide suggestions
//    });
//});