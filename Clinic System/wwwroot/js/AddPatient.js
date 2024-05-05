document.getElementById('addChronicDisease').addEventListener('click', function () {
	const chronicDiseasesContainer = document.getElementById('chronicDiseasesContainer'); // This is where we'll add the new section
	const chronicDiseaseCount = document.querySelectorAll('.ChronicDisease-input').length; // Counts existing inputs

	const newChronicDiseaseDiv = document.createElement('div');
	newChronicDiseaseDiv.classList.add('mb-3');

	newChronicDiseaseDiv.innerHTML = `
        <label class="form-label" style="float: inline-start">اسم المرض المزمن</label>
        <div class="input-group mb-2">
            <input type="text" required name="patientChronicDiseases[${chronicDiseaseCount}].ChronicDisease" class="form-control ChronicDisease-input" placeholder="ادخل الاسم" />
            <button class="btn btn-danger remove-ChronicDisease" type="button">إزالة</button>
        </div>
        <div class="validation-message" style="display:none; color:red;">مطلوب.</div>
    `;

	// Append the new div to the container
	chronicDiseasesContainer.appendChild(newChronicDiseaseDiv);

	// Add click event to the "Remove" button
	newChronicDiseaseDiv.querySelector('.remove-ChronicDisease').addEventListener('click', function () {
		newChronicDiseaseDiv.remove(); // Removes the entire section
	});
});
document.getElementById('addSpecialCase').addEventListener('click', function () {
	const specialCasesContainer = document.getElementById('specialCasesContainer'); // Container for new sections
	const specialCaseCount = document.querySelectorAll('.SpecialCase-input').length;

	const newSpecialCaseDiv = document.createElement('div');
	newSpecialCaseDiv.classList.add('mb-3');

	newSpecialCaseDiv.innerHTML = `
        <label class="form-label" style="float: inline-start;">اسم الحالة الخاصة</label>
        <div class="input-group mb-2">
            <input type="text" required name="patientSpecialCase[${specialCaseCount}].SpecialCase" class="form-control SpecialCase-input" placeholder="ادخل الاسم" required />
            <button class="btn btn-danger remove-SpecialCase" type="button">إزالة</button>
        </div>
        <div class="validation-message" style="display:none; color:red;">مطلوب.</div>
    `;

	specialCasesContainer.appendChild(newSpecialCaseDiv);

	// Add remove functionality for the new special case section
	newSpecialCaseDiv.querySelector('.remove-SpecialCase').addEventListener('click', function () {
		newSpecialCaseDiv.remove();
	});
});





