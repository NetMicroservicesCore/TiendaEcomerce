        // Función para cambiar entre vistas
    function showView(view) {
        document.getElementById('loginView').style.display = 'none';
    document.getElementById('registerView').style.display = 'none';
    document.getElementById('recoverView').style.display = 'none';

    if (view === 'login') {
        document.getElementById('loginView').style.display = 'block';
            } else if (view === 'register') {
        document.getElementById('registerView').style.display = 'block';
            } else if (view === 'recover') {
        document.getElementById('recoverView').style.display = 'block';
            }
        }

    // Validación del formulario de login
    document.getElementById('loginForm').addEventListener('submit', function(e) {
        e.preventDefault();

    if (!this.checkValidity()) {
        e.stopPropagation();
    this.classList.add('was-validated');
            } else {
        alert('¡Inicio de sesión exitoso! (Simulación)');
    this.classList.remove('was-validated');
    this.reset();
            }
        });

    // Validación del formulario de registro
    document.getElementById('registerForm').addEventListener('submit', function(e) {
        e.preventDefault();

    const password = document.getElementById('registerPassword').value;
    const confirmPassword = document.getElementById('registerPasswordConfirm').value;
    const confirmInput = document.getElementById('registerPasswordConfirm');

    // Validar que las contraseñas coincidan
    if (password !== confirmPassword) {
        confirmInput.setCustomValidity('Las contraseñas no coinciden');
            } else {
        confirmInput.setCustomValidity('');
            }

    if (!this.checkValidity()) {
        e.stopPropagation();
    this.classList.add('was-validated');
            } else {
        alert('¡Cuenta creada exitosamente! (Simulación)');
    this.classList.remove('was-validated');
    this.reset();
    showView('login');
            }
        });

    // Validación del formulario de recuperación
    document.getElementById('recoverForm').addEventListener('submit', function(e) {
        e.preventDefault();

    if (!this.checkValidity()) {
        e.stopPropagation();
    this.classList.add('was-validated');
            } else {
        alert('¡Instrucciones enviadas a tu correo! (Simulación)');
    this.classList.remove('was-validated');
    this.reset();
    showView('login');
            }
        });

    // Validación en tiempo real para las contraseñas
    document.getElementById('registerPasswordConfirm').addEventListener('input', function() {
            const password = document.getElementById('registerPassword').value;
    if (this.value !== password) {
        this.setCustomValidity('Las contraseñas no coinciden');
            } else {
        this.setCustomValidity('');
            }
        });
