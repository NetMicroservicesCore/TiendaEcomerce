    let currentStep = 1;
    const totalSteps = 4;

    // Función para cambiar de paso
    function goToStep(step) {
        // Ocultar todas las secciones
        document.querySelectorAll('.form-section').forEach(section => {
            section.classList.remove('active');
        });

    // Mostrar la sección actual
    document.querySelector(`[data-section="${step}"]`).classList.add('active');

            // Actualizar indicadores de paso
            document.querySelectorAll('.step').forEach((stepEl, index) => {
                const stepNum = index + 1;
    stepEl.classList.remove('active', 'completed');

    if (stepNum < step) {
        stepEl.classList.add('completed');
                } else if (stepNum === step) {
        stepEl.classList.add('active');
                }
            });

    // Actualizar botones
    document.getElementById('btnPrev').style.display = step === 1 ? 'none' : 'inline-block';
    document.getElementById('btnNext').style.display = step === totalSteps ? 'none' : 'inline-block';
    document.getElementById('btnSubmit').style.display = step === totalSteps ? 'inline-block' : 'none';

    // Si estamos en el último paso, mostrar resumen
    if (step === totalSteps) {
        showSummary();
            }

    currentStep = step;
    window.scrollTo({top: 0, behavior: 'smooth' });
        }

    // Validar sección actual
    function validateCurrentSection() {
            const currentSection = document.querySelector(`[data-section="${currentStep}"]`);
    const inputs = currentSection.querySelectorAll('input[required], select[required], textarea[required]');
    let isValid = true;

            inputs.forEach(input => {
                if (!input.checkValidity()) {
        isValid = false;
    input.classList.add('is-invalid');
                } else {
        input.classList.remove('is-invalid');
    input.classList.add('is-valid');
                }
            });

    return isValid;
        }

    // Mostrar resumen de datos
    function showSummary() {
            const summary = document.getElementById('summaryData');
    const html = `
    <div class="card">
        <div class="card-body">
            <h6 class="card-subtitle mb-3 text-muted"><i class="bi bi-person-badge"></i> Información Personal</h6>
            <p class="mb-1"><strong>Nombre completo:</strong> ${document.getElementById('firstName').value} ${document.getElementById('lastName').value}</p>
            <p class="mb-1"><strong>Fecha de nacimiento:</strong> ${document.getElementById('birthDate').value}</p>
            <p class="mb-1"><strong>Género:</strong> ${document.getElementById('gender').value}</p>
            <p class="mb-1"><strong>Nacionalidad:</strong> ${document.getElementById('nationality').value}</p>
            <p class="mb-3"><strong>Identificación:</strong> ${document.getElementById('idNumber').value}</p>

            <h6 class="card-subtitle mb-3 text-muted"><i class="bi bi-envelope"></i> Contacto</h6>
            <p class="mb-1"><strong>Email:</strong> ${document.getElementById('email').value}</p>
            <p class="mb-1"><strong>Teléfono:</strong> ${document.getElementById('phone').value}</p>
            ${document.getElementById('phoneAlt').value ? `<p class="mb-1"><strong>Teléfono alternativo:</strong> ${document.getElementById('phoneAlt').value}</p>` : ''}
            <p class="mb-3"><strong>Preferencia de contacto:</strong> ${document.getElementById('contactPreference').value}</p>

            <h6 class="card-subtitle mb-3 text-muted"><i class="bi bi-geo-alt"></i> Dirección</h6>
            <p class="mb-1"><strong>Dirección:</strong> ${document.getElementById('street').value}</p>
            <p class="mb-1"><strong>Colonia:</strong> ${document.getElementById('neighborhood').value}, CP ${document.getElementById('postalCode').value}</p>
            <p class="mb-1"><strong>Ciudad:</strong> ${document.getElementById('city').value}, ${document.getElementById('state').value}</p>
            <p class="mb-0"><strong>País:</strong> ${document.getElementById('country').value}</p>
        </div>
    </div>
    `;
    summary.innerHTML = html;
        }

    // Botón Siguiente
    document.getElementById('btnNext').addEventListener('click', function() {
            if (validateCurrentSection()) {
                if (currentStep < totalSteps) {
        goToStep(currentStep + 1);
                }
            }
        });

    // Botón Anterior
    document.getElementById('btnPrev').addEventListener('click', function() {
            if (currentStep > 1) {
        goToStep(currentStep - 1);
            }
        });

    // Validación de correos coincidentes
    document.getElementById('emailConfirm').addEventListener('input', function() {
            const email = document.getElementById('email').value;
    if (this.value !== email) {
        this.setCustomValidity('Los correos no coinciden');
            } else {
        this.setCustomValidity('');
            }
        });

    // Enviar formulario
    document.getElementById('userDataForm').addEventListener('submit', function(e) {
        e.preventDefault();

    if (!this.checkValidity()) {
        e.stopPropagation();
    this.classList.add('was-validated');
            } else {
        // Simular envío exitoso
        alert('¡Registro completado exitosamente!\n\nTus datos han sido guardados correctamente.');
    this.reset();
    this.classList.remove('was-validated');
    goToStep(1);
            }
        });

        // Validación en tiempo real para remover mensajes de error
        document.querySelectorAll('input, select, textarea').forEach(input => {
        input.addEventListener('input', function () {
            if (this.checkValidity()) {
                this.classList.remove('is-invalid');
                this.classList.add('is-valid');
            }
        });
        });

