// Criminal Case Management System - Custom JavaScript

document.addEventListener('DOMContentLoaded', function() {
    // Initialize tooltips
    var tooltipTriggerList = [].slice.call(document.querySelectorAll('[data-bs-toggle="tooltip"]'));
    var tooltipList = tooltipTriggerList.map(function (tooltipTriggerEl) {
        return new bootstrap.Tooltip(tooltipTriggerEl);
    });

    // Initialize popovers
    var popoverTriggerList = [].slice.call(document.querySelectorAll('[data-bs-toggle="popover"]'));
    var popoverList = popoverTriggerList.map(function (popoverTriggerEl) {
        return new bootstrap.Popover(popoverTriggerEl);
    });

    // Auto-hide alerts after 5 seconds
    setTimeout(function() {
        var alerts = document.querySelectorAll('.alert');
        alerts.forEach(function(alert) {
            var bsAlert = new bootstrap.Alert(alert);
            bsAlert.close();
        });
    }, 5000);

    // Confirm delete actions
    var deleteButtons = document.querySelectorAll('.btn-delete, [data-confirm="delete"]');
    deleteButtons.forEach(function(button) {
        button.addEventListener('click', function(e) {
            if (!confirm('هل أنت متأكد من حذف هذا العنصر؟')) {
                e.preventDefault();
                return false;
            }
        });
    });

    // Form validation enhancement
    var forms = document.querySelectorAll('.needs-validation');
    forms.forEach(function(form) {
        form.addEventListener('submit', function(event) {
            if (!form.checkValidity()) {
                event.preventDefault();
                event.stopPropagation();
            }
            form.classList.add('was-validated');
        });
    });

    // Search functionality
    var searchInput = document.querySelector('#searchInput');
    if (searchInput) {
        searchInput.addEventListener('input', function() {
            var searchTerm = this.value.toLowerCase();
            var tableRows = document.querySelectorAll('tbody tr');
            
            tableRows.forEach(function(row) {
                var text = row.textContent.toLowerCase();
                if (text.includes(searchTerm)) {
                    row.style.display = '';
                } else {
                    row.style.display = 'none';
                }
            });
        });
    }

    // File upload preview
    var fileInputs = document.querySelectorAll('input[type="file"]');
    fileInputs.forEach(function(input) {
        input.addEventListener('change', function() {
            var files = this.files;
            var previewContainer = this.parentElement.querySelector('.file-preview');
            
            if (!previewContainer) {
                previewContainer = document.createElement('div');
                previewContainer.className = 'file-preview mt-2';
                this.parentElement.appendChild(previewContainer);
            }
            
            previewContainer.innerHTML = '';
            
            for (var i = 0; i < files.length; i++) {
                var file = files[i];
                var fileDiv = document.createElement('div');
                fileDiv.className = 'file-item d-flex align-items-center mb-2';
                
                var icon = document.createElement('i');
                icon.className = getFileIcon(file.type);
                icon.style.marginLeft = '0.5rem';
                
                var fileName = document.createElement('span');
                fileName.textContent = file.name;
                
                var fileSize = document.createElement('small');
                fileSize.className = 'text-muted ms-2';
                fileSize.textContent = formatFileSize(file.size);
                
                fileDiv.appendChild(icon);
                fileDiv.appendChild(fileName);
                fileDiv.appendChild(fileSize);
                previewContainer.appendChild(fileDiv);
            }
        });
    });

    // Date picker enhancement
    var dateInputs = document.querySelectorAll('input[type="date"]');
    dateInputs.forEach(function(input) {
        if (!input.value) {
            input.value = new Date().toISOString().split('T')[0];
        }
    });

    // Table row hover effect
    var tableRows = document.querySelectorAll('tbody tr');
    tableRows.forEach(function(row) {
        row.addEventListener('mouseenter', function() {
            this.style.backgroundColor = '#f8f9fa';
        });
        
        row.addEventListener('mouseleave', function() {
            this.style.backgroundColor = '';
        });
    });

    // Loading spinner for form submissions
    var forms = document.querySelectorAll('form');
    forms.forEach(function(form) {
        form.addEventListener('submit', function() {
            var submitButton = form.querySelector('button[type="submit"]');
            if (submitButton) {
                var originalText = submitButton.innerHTML;
                submitButton.innerHTML = '<span class="spinner-border spinner-border-sm me-2"></span>جاري الحفظ...';
                submitButton.disabled = true;
                
                // Reset button after 10 seconds (fallback)
                setTimeout(function() {
                    submitButton.innerHTML = originalText;
                    submitButton.disabled = false;
                }, 10000);
            }
        });
    });

    // Auto-save functionality for textareas
    var textareas = document.querySelectorAll('textarea');
    textareas.forEach(function(textarea) {
        var key = 'autosave_' + textarea.name;
        var savedValue = localStorage.getItem(key);
        
        if (savedValue && !textarea.value) {
            textarea.value = savedValue;
        }
        
        textarea.addEventListener('input', function() {
            localStorage.setItem(key, this.value);
        });
    });

    // Print functionality
    var printButtons = document.querySelectorAll('.btn-print');
    printButtons.forEach(function(button) {
        button.addEventListener('click', function() {
            window.print();
        });
    });

    // Export functionality
    var exportButtons = document.querySelectorAll('.btn-export');
    exportButtons.forEach(function(button) {
        button.addEventListener('click', function() {
            var format = this.dataset.format || 'pdf';
            var table = document.querySelector('table');
            
            if (table) {
                exportTable(table, format);
            }
        });
    });

    // Notification system
    window.showNotification = function(message, type = 'info') {
        var notification = document.createElement('div');
        notification.className = 'alert alert-' + type + ' alert-dismissible fade show position-fixed';
        notification.style.cssText = 'top: 20px; right: 20px; z-index: 9999; min-width: 300px;';
        notification.innerHTML = `
            ${message}
            <button type="button" class="btn-close" data-bs-dismiss="alert"></button>
        `;
        
        document.body.appendChild(notification);
        
        setTimeout(function() {
            if (notification.parentNode) {
                notification.remove();
            }
        }, 5000);
    };

    // AJAX helper function
    window.ajaxRequest = function(url, method = 'GET', data = null) {
        return new Promise(function(resolve, reject) {
            var xhr = new XMLHttpRequest();
            xhr.open(method, url, true);
            xhr.setRequestHeader('Content-Type', 'application/json');
            xhr.setRequestHeader('X-Requested-With', 'XMLHttpRequest');
            
            xhr.onload = function() {
                if (xhr.status >= 200 && xhr.status < 300) {
                    try {
                        var response = JSON.parse(xhr.responseText);
                        resolve(response);
                    } catch (e) {
                        resolve(xhr.responseText);
                    }
                } else {
                    reject(xhr.statusText);
                }
            };
            
            xhr.onerror = function() {
                reject(xhr.statusText);
            };
            
            if (data) {
                xhr.send(JSON.stringify(data));
            } else {
                xhr.send();
            }
        });
    };
});

// Helper functions
function getFileIcon(mimeType) {
    if (mimeType.startsWith('image/')) {
        return 'fas fa-image text-primary';
    } else if (mimeType === 'application/pdf') {
        return 'fas fa-file-pdf text-danger';
    } else if (mimeType.includes('word')) {
        return 'fas fa-file-word text-primary';
    } else if (mimeType.includes('excel')) {
        return 'fas fa-file-excel text-success';
    } else {
        return 'fas fa-file text-secondary';
    }
}

function formatFileSize(bytes) {
    if (bytes === 0) return '0 Bytes';
    var k = 1024;
    var sizes = ['Bytes', 'KB', 'MB', 'GB'];
    var i = Math.floor(Math.log(bytes) / Math.log(k));
    return parseFloat((bytes / Math.pow(k, i)).toFixed(2)) + ' ' + sizes[i];
}

function exportTable(table, format) {
    // Simple CSV export implementation
    if (format === 'csv') {
        var csv = [];
        var rows = table.querySelectorAll('tr');
        
        for (var i = 0; i < rows.length; i++) {
            var row = [], cols = rows[i].querySelectorAll('td, th');
            
            for (var j = 0; j < cols.length; j++) {
                var text = cols[j].innerText.replace(/"/g, '""');
                row.push('"' + text + '"');
            }
            
            csv.push(row.join(','));
        }
        
        var csvContent = 'data:text/csv;charset=utf-8,' + csv.join('\n');
        var encodedUri = encodeURI(csvContent);
        var link = document.createElement('a');
        link.setAttribute('href', encodedUri);
        link.setAttribute('download', 'export.csv');
        document.body.appendChild(link);
        link.click();
        document.body.removeChild(link);
    }
}

// Data table functionality
function initializeDataTable(tableId) {
    var table = document.getElementById(tableId);
    if (!table) return;
    
    // Add search functionality
    var searchInput = document.createElement('input');
    searchInput.type = 'text';
    searchInput.className = 'form-control mb-3';
    searchInput.placeholder = 'البحث...';
    table.parentNode.insertBefore(searchInput, table);
    
    searchInput.addEventListener('input', function() {
        var searchTerm = this.value.toLowerCase();
        var rows = table.querySelectorAll('tbody tr');
        
        rows.forEach(function(row) {
            var text = row.textContent.toLowerCase();
            row.style.display = text.includes(searchTerm) ? '' : 'none';
        });
    });
}

// Chart configuration
function initializeCharts() {
    // Chart.js global configuration
    Chart.defaults.font.family = 'Segoe UI, Tahoma, Geneva, Verdana, sans-serif';
    Chart.defaults.color = '#5a5c69';
    Chart.defaults.plugins.legend.position = 'bottom';
    Chart.defaults.plugins.legend.labels.usePointStyle = true;
}

// Initialize charts when DOM is loaded
document.addEventListener('DOMContentLoaded', function() {
    initializeCharts();
});
