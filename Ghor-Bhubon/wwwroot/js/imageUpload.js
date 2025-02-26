document.addEventListener("DOMContentLoaded", function () {
    const imageInput = document.getElementById("imageInput");
    const imageContainer = document.getElementById("imageContainer");
    const uploadedImagesInput = document.getElementById("uploadedImages");

    let uploadedImagePaths = []; // Store image paths

    imageInput.addEventListener("change", function (event) {
        const files = event.target.files;
        if (files.length === 0) return;

        for (let file of files) {
            uploadSingleImage(file);
        }

        imageInput.value = ""; // Reset input
    });

    function uploadSingleImage(file) {
        const formData = new FormData();
        formData.append("Image", file);

        fetch("/Landlord/AddSingleImage", {
            method: "POST",
            body: formData
        })
            .then(response => response.json())
            .then(data => {
                if (data.success) {
                    uploadedImagePaths.push(data.imagePath);
                    uploadedImagesInput.value = uploadedImagePaths.join(","); // Update hidden input
                    displayImage(data.imagePath);
                } else {
                    console.error("Upload failed:", data.error);
                }
            })
            .catch(error => console.error("Error:", error));
    }

    function displayImage(imagePath) {
        const imageWrapper = document.createElement("div");
        imageWrapper.classList.add("image-wrapper");

        const img = document.createElement("img");
        img.src = imagePath;

        const removeBtn = document.createElement("button");
        removeBtn.innerHTML = "×";
        removeBtn.classList.add("remove-btn");
        removeBtn.onclick = function () {
            imageWrapper.remove();
            uploadedImagePaths = uploadedImagePaths.filter(path => path !== imagePath);
            uploadedImagesInput.value = uploadedImagePaths.join(","); // Update hidden input
        };

        imageWrapper.appendChild(img);
        imageWrapper.appendChild(removeBtn);
        imageContainer.appendChild(imageWrapper);
    }
});
