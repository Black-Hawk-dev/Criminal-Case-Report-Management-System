// Criminal Case Management System - Custom JavaScript

// Global variables
let currentUser = null;
let isAuthenticated = false;

// Initialize when document is ready
document.addEventListener('DOMContentLoaded', function() {
    initializeSystem();
    setupEventListeners();
    setupFormValidation();
    setupDataTables();
    setupCharts();
});

// Initialize system
function initializeSystem() {
    console.log('Initializing Criminal Case Management System...');
    
    // Check authentication status
    checkAuthenticationStatus();
    
    // Setup tooltips
    setupTooltips();
    
    // Setup modals
    setupModals();
    
    // Setup notifications
    setupNotifications();
}

// Setup event listeners
function setupEventListeners() {
    // Search functionality
    const searchInputs = document.querySelectorAll('.search-input');
    searchInputs.forEach(input => {
        input.addEventListener('input', debounce(handleSearch, 300));
    });
    
    // Form submissions
    const forms = document.querySelectorAll('form[data-ajax="true"]');
    forms.forEach(form => {
        form.addEventListener('submit', handleAjaxFormSubmit);
    });
    
    // File uploads
    const fileInputs = document.querySelectorAll('input[type="file"]');
    fileInputs.forEach(input => {
        input.addEventListener('change', handleFileUpload);
    });
    
    // Delete confirmations
    const deleteButtons = document.querySelectorAll('.btn-delete');
    deleteButtons.forEach(button => {
        button.addEventListener('click', handleDeleteConfirmation);
    });
}

// Setup form validation
function setupFormValidation() {
    const forms = document.querySelectorAll('form[data-validate="true"]');
    forms.forEach(form => {
        form.addEventListener('submit', validateForm);
    });
}

// Setup data tables
function setupDataTables() {
    const tables = document.querySelectorAll('.data-table');
    tables.forEach(table => {
        if (typeof DataTable !== 'undefined') {
            new DataTable(table, {
                language: {
                    url: '/js/datatables-ar.json'
                },
                responsive: true,
                pageLength: 10,
                order: [[0, 'desc']]
            });
        }
    });
}

// Setup charts
function setupCharts() {
    // This will be handled by Chart.js in individual pages
    console.log('Charts setup completed');
}

// Setup tooltips
function setupTooltips() {
    const tooltipTriggerList = [].slice.call(document.querySelectorAll('[data-bs-toggle="tooltip"]'));
    tooltipTriggerList.map(function (tooltipTriggerEl) {
        return new bootstrap.Tooltip(tooltipTriggerEl);
    });
}

// Setup modals
function setupModals() {
    const modalTriggerList = [].slice.call(document.querySelectorAll('[data-bs-toggle="modal"]'));
    modalTriggerList.map(function (modalTriggerEl) {
        return new bootstrap.Modal(modalTriggerEl);
    });
}

// Setup notifications
function setupNotifications() {
    // Auto-hide alerts after 5 seconds
    const alerts = document.querySelectorAll('.alert:not(.alert-permanent)');
    alerts.forEach(alert => {
        setTimeout(() => {
            if (alert.parentNode) {
                alert.style.transition = 'opacity 0.5s';
                alert.style.opacity = '0';
                setTimeout(() => {
                    if (alert.parentNode) {
                        alert.remove();
                    }
                }, 500);
            }
        }, 5000);
    });
}

// Check authentication status
function checkAuthenticationStatus() {
    // This would typically check with the server
    const authToken = localStorage.getItem('authToken');
    isAuthenticated = !!authToken;
    
    if (isAuthenticated) {
        document.body.classList.add('authenticated');
    } else {
        document.body.classList.add('unauthenticated');
    }
}

// Handle search functionality
function handleSearch(event) {
    const searchTerm = event.target.value;
    const searchType = event.target.dataset.searchType || 'general';
    
    // Implement search logic based on type
    switch (searchType) {
        case 'reports':
            searchReports(searchTerm);
            break;
        case 'cases':
            searchCases(searchTerm);
            break;
        case 'suspects':
            searchSuspects(searchTerm);
            break;
        default:
            console.log('Searching for:', searchTerm);
    }
}

// Search reports
function searchReports(searchTerm) {
    // Implement AJAX search for reports
    fetch(`/Reports/Search?term=${encodeURIComponent(searchTerm)}`)
        .then(response => response.json())
        .then(data => {
            updateReportsTable(data);
        })
        .catch(error => {
            console.error('Search error:', error);
            showNotification('خطأ في البحث', 'error');
        });
}

// Search cases
function searchCases(searchTerm) {
    // Implement AJAX search for cases
    fetch(`/Cases/Search?term=${encodeURIComponent(searchTerm)}`)
        .then(response => response.json())
        .then(data => {
            updateCasesTable(data);
        })
        .catch(error => {
            console.error('Search error:', error);
            showNotification('خطأ في البحث', 'error');
        });
}

// Search suspects
function searchSuspects(searchTerm) {
    // Implement AJAX search for suspects
    fetch(`/Suspects/Search?term=${encodeURIComponent(searchTerm)}`)
        .then(response => response.json())
        .then(data => {
            updateSuspectsTable(data);
        })
        .catch(error => {
            console.error('Search error:', error);
            showNotification('خطأ في البحث', 'error');
        });
}

// Handle AJAX form submissions
function handleAjaxFormSubmit(event) {
    event.preventDefault();
    
    const form = event.target;
    const formData = new FormData(form);
    const url = form.action;
    const method = form.method;
    
    // Show loading state
    const submitButton = form.querySelector('button[type="submit"]');
    const originalText = submitButton.innerHTML;
    submitButton.innerHTML = '<span class="spinner-border spinner-border-sm me-2"></span>جاري الحفظ...';
    submitButton.disabled = true;
    
    fetch(url, {
        method: method,
        body: formData
    })
    .then(response => response.json())
    .then(data => {
        if (data.success) {
            showNotification(data.message || 'تم الحفظ بنجاح', 'success');
            if (data.redirect) {
                window.location.href = data.redirect;
            }
        } else {
            showNotification(data.message || 'حدث خطأ أثناء الحفظ', 'error');
        }
    })
    .catch(error => {
        console.error('Form submission error:', error);
        showNotification('حدث خطأ في الاتصال', 'error');
    })
    .finally(() => {
        // Restore button state
        submitButton.innerHTML = originalText;
        submitButton.disabled = false;
    });
}

// Handle file uploads
function handleFileUpload(event) {
    const files = event.target.files;
    const maxSize = 10 * 1024 * 1024; // 10MB
    const allowedTypes = ['image/jpeg', 'image/png', 'image/gif', 'application/pdf', 'application/msword', 'application/vnd.openxmlformats-officedocument.wordprocessingml.document'];
    
    for (let i = 0; i < files.length; i++) {
        const file = files[i];
        
        // Check file size
        if (file.size > maxSize) {
            showNotification(`الملف "${file.name}" أكبر من 10 ميجابايت`, 'warning');
            event.target.value = '';
            return;
        }
        
        // Check file type
        if (!allowedTypes.includes(file.type)) {
            showNotification(`نوع الملف "${file.name}" غير مسموح به`, 'warning');
            event.target.value = '';
            return;
        }
    }
    
    // Show file preview if images
    showFilePreview(files);
}

// Show file preview
function showFilePreview(files) {
    const previewContainer = document.querySelector('.file-preview');
    if (!previewContainer) return;
    
    previewContainer.innerHTML = '';
    
    Array.from(files).forEach(file => {
        if (file.type.startsWith('image/')) {
            const reader = new FileReader();
            reader.onload = function(e) {
                const img = document.createElement('img');
                img.src = e.target.result;
                img.className = 'img-thumbnail me-2 mb-2';
                img.style.maxWidth = '100px';
                img.style.maxHeight = '100px';
                previewContainer.appendChild(img);
            };
            reader.readAsDataURL(file);
        } else {
            const fileInfo = document.createElement('div');
            fileInfo.className = 'badge bg-secondary me-2 mb-2';
            fileInfo.textContent = file.name;
            previewContainer.appendChild(fileInfo);
        }
    });
}

// Handle delete confirmation
function handleDeleteConfirmation(event) {
    event.preventDefault();
    
    const confirmMessage = event.target.dataset.confirmMessage || 'هل أنت متأكد من الحذف؟';
    
    if (confirm(confirmMessage)) {
        const url = event.target.href;
        
        fetch(url, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
                'RequestVerificationToken': document.querySelector('input[name="__RequestVerificationToken"]').value
            }
        })
        .then(response => response.json())
        .then(data => {
            if (data.success) {
                showNotification(data.message || 'تم الحذف بنجاح', 'success');
                // Remove the row from table or redirect
                if (data.redirect) {
                    window.location.href = data.redirect;
                } else {
                    const row = event.target.closest('tr');
                    if (row) {
                        row.remove();
                    }
                }
            } else {
                showNotification(data.message || 'حدث خطأ أثناء الحذف', 'error');
            }
        })
        .catch(error => {
            console.error('Delete error:', error);
            showNotification('حدث خطأ في الاتصال', 'error');
        });
    }
}

// Validate form
function validateForm(event) {
    const form = event.target;
    const inputs = form.querySelectorAll('input[required], select[required], textarea[required]');
    let isValid = true;
    
    inputs.forEach(input => {
        if (!input.value.trim()) {
            isValid = false;
            input.classList.add('is-invalid');
            
            // Add error message
            let errorDiv = input.parentNode.querySelector('.invalid-feedback');
            if (!errorDiv) {
                errorDiv = document.createElement('div');
                errorDiv.className = 'invalid-feedback';
                input.parentNode.appendChild(errorDiv);
            }
            errorDiv.textContent = 'هذا الحقل مطلوب';
        } else {
            input.classList.remove('is-invalid');
            const errorDiv = input.parentNode.querySelector('.invalid-feedback');
            if (errorDiv) {
                errorDiv.remove();
            }
        }
    });
    
    if (!isValid) {
        event.preventDefault();
        showNotification('يرجى إكمال جميع الحقول المطلوبة', 'warning');
    }
    
    return isValid;
}

// Show notification
function showNotification(message, type = 'info') {
    const alertClass = `alert-${type}`;
    const iconClass = getNotificationIcon(type);
    
    const notification = document.createElement('div');
    notification.className = `alert ${alertClass} alert-dismissible fade show position-fixed`;
    notification.style.cssText = 'top: 20px; right: 20px; z-index: 9999; min-width: 300px;';
    notification.innerHTML = `
        <i class="${iconClass} me-2"></i>
        ${message}
        <button type="button" class="btn-close" data-bs-dismiss="alert"></button>
    `;
    
    document.body.appendChild(notification);
    
    // Auto-remove after 5 seconds
    setTimeout(() => {
        if (notification.parentNode) {
            notification.remove();
        }
    }, 5000);
}

// Get notification icon
function getNotificationIcon(type) {
    switch (type) {
        case 'success':
            return 'fas fa-check-circle';
        case 'error':
            return 'fas fa-exclamation-circle';
        case 'warning':
            return 'fas fa-exclamation-triangle';
        default:
            return 'fas fa-info-circle';
    }
}

// Update reports table
function updateReportsTable(data) {
    const tbody = document.querySelector('#reportsTable tbody');
    if (!tbody) return;
    
    tbody.innerHTML = '';
    
    data.forEach(report => {
        const row = document.createElement('tr');
        row.innerHTML = `
            <td>${report.id}</td>
            <td>${report.reporterName}</td>
            <td>${report.reporterIdNumber}</td>
            <td><span class="badge bg-primary">${report.type}</span></td>
            <td>${formatDate(report.reportDate)}</td>
            <td><span class="badge bg-success">${report.status}</span></td>
            <td>${report.createdByFullName}</td>
            <td>
                <div class="btn-group">
                    <a href="/Reports/Details/${report.id}" class="btn btn-sm btn-info"><i class="fas fa-eye"></i></a>
                    <a href="/Reports/Edit/${report.id}" class="btn btn-sm btn-warning"><i class="fas fa-edit"></i></a>
                    <a href="/Reports/Delete/${report.id}" class="btn btn-sm btn-danger"><i class="fas fa-trash"></i></a>
                </div>
            </td>
        `;
        tbody.appendChild(row);
    });
}

// Update cases table
function updateCasesTable(data) {
    // Similar to updateReportsTable but for cases
    console.log('Updating cases table with:', data);
}

// Update suspects table
function updateSuspectsTable(data) {
    // Similar to updateReportsTable but for suspects
    console.log('Updating suspects table with:', data);
}

// Format date
function formatDate(dateString) {
    const date = new Date(dateString);
    return date.toLocaleDateString('ar-SA');
}

// Debounce function
function debounce(func, wait) {
    let timeout;
    return function executedFunction(...args) {
        const later = () => {
            clearTimeout(timeout);
            func(...args);
        };
        clearTimeout(timeout);
        timeout = setTimeout(later, wait);
    };
}

// Export functions for global use
window.CriminalCaseManagement = {
    showNotification,
    handleSearch,
    validateForm,
    formatDate
};