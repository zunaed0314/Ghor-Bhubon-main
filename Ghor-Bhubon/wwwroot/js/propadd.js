const connection = new signalR.HubConnectionBuilder()
    .withUrl("/propertyHub") // Use propertyHub for property updates
    .build();

connection.on("NewPropertyAdded", function (newProperty) {
    // Create a new property card dynamically
    const propertyGrid = document.getElementById("propertyGrid");
    const propertyDiv = document.createElement("div");
    propertyDiv.classList.add("property");
    propertyDiv.id = "property-" + newProperty.flatID;

    // Property image
    const propertyImageDiv = document.createElement("div");
    propertyImageDiv.classList.add("property-images");
    const img = document.createElement("img");
    img.src = newProperty.imagePaths.split(",")[0].trim();
    img.alt = "Property Image";
    img.classList.add("property-image");
    propertyImageDiv.appendChild(img);

    // Property details
    const h3 = document.createElement("h3");
    h3.innerText = newProperty.area + ", " + newProperty.city;
    const rentPara = document.createElement("p");
    rentPara.innerHTML = `<strong>Rent:</strong> ${newProperty.rent} TK`;
    const roomsPara = document.createElement("p");
    roomsPara.innerHTML = `<strong>Rooms:</strong> ${newProperty.numberOfRooms} | <strong>Bathrooms:</strong> ${newProperty.numberOfBathrooms}`;

    const viewMoreLink = document.createElement("a");
    viewMoreLink.href = `@Url.Action("PropertyDetails", "Landlord", new { id = "" })`.replace('""', newProperty.flatID);
    viewMoreLink.classList.add("view-more-btn");
    viewMoreLink.innerText = "View More Details";

    // Append new elements to the property card
    propertyDiv.appendChild(propertyImageDiv);
    propertyDiv.appendChild(h3);
    propertyDiv.appendChild(rentPara);
    propertyDiv.appendChild(roomsPara);
    propertyDiv.appendChild(viewMoreLink);

    // Append the new property card to the property grid
    propertyGrid.appendChild(propertyDiv);
});

connection.start().catch(function (err) {
    return console.error(err.toString());
});
