// SignalR News Real-time Updates
class NewsSignalR {
    constructor() {
        this.connection = null;
        this.isConnected = false;
        this.init();
    }

    async init() {
        try {
            // Check if SignalR is available
            if (typeof signalR === 'undefined') {
                console.error("SignalR library not loaded");
                this.showNotification("SignalR library not available", "danger");
                return;
            }

            // Create SignalR connection
            this.connection = new signalR.HubConnectionBuilder()
                .withUrl("/newsHub")
                .withAutomaticReconnect()
                .configureLogging(signalR.LogLevel.Information)
                .build();

            // Set up event handlers
            this.setupEventHandlers();

            // Start connection
            await this.connection.start();
            this.isConnected = true;
            console.log("SignalR Connected successfully");

            // Show connection status
            this.showNotification("Connected to real-time updates", "success");

        } catch (err) {
            console.error("SignalR Connection failed: ", err);
            this.showNotification("Failed to connect to real-time updates", "warning");

            // Retry connection after 5 seconds
            setTimeout(() => this.init(), 5000);
        }
    }

    setupEventHandlers() {
        // Handle connection confirmation
        this.connection.on("Connected", (data) => {
            console.log("Hub Connected:", data);
            this.showNotification("Real-time updates enabled", "success");
        });

        // Handle news created
        this.connection.on("NewsCreated", (data) => {
            console.log("News Created:", data);
            this.showNotification(data.Message, "success");
            this.refreshNewsList();
        });

        // Handle news updated
        this.connection.on("NewsUpdated", (data) => {
            console.log("News Updated:", data);
            this.showNotification(data.Message, "info");
            this.refreshNewsList();
        });

        // Handle news deleted
        this.connection.on("NewsDeleted", (data) => {
            console.log("News Deleted:", data);
            this.showNotification(data.Message, "warning");
            this.refreshNewsList();
        });

        // Handle test messages
        this.connection.on("TestMessage", (data) => {
            console.log("Test Message:", data);
            this.showNotification(`Test: ${data.Message}`, "info");
        });

        // Handle connection events
        this.connection.onreconnecting((error) => {
            console.log("SignalR Reconnecting:", error);
            this.isConnected = false;
            this.showNotification("Reconnecting to real-time updates...", "warning");
        });

        this.connection.onreconnected((connectionId) => {
            console.log("SignalR Reconnected:", connectionId);
            this.isConnected = true;
            this.showNotification("Reconnected to real-time updates", "success");
        });

        this.connection.onclose((error) => {
            console.log("SignalR Connection closed:", error);
            this.isConnected = false;
            this.showNotification("Disconnected from real-time updates", "danger");
        });
    }

    showNotification(message, type = "info") {
        // Create notification element
        const notification = document.createElement('div');
        notification.className = `alert alert-${type} alert-dismissible fade show position-fixed`;
        notification.style.cssText = 'top: 20px; right: 20px; z-index: 9999; min-width: 300px;';
        notification.innerHTML = `
            <i class="bi bi-broadcast"></i> ${message}
            <button type="button" class="btn-close" data-bs-dismiss="alert"></button>
        `;

        // Add to page
        document.body.appendChild(notification);

        // Auto remove after 5 seconds
        setTimeout(() => {
            if (notification.parentNode) {
                notification.remove();
            }
        }, 5000);
    }

    refreshNewsList() {
        // Check if we're on a news-related page
        const currentPath = window.location.pathname.toLowerCase();
        
        if (currentPath.includes('/news') || currentPath.includes('/staff')) {
            // Refresh the page content without full page reload
            this.refreshNewsContent();
        }
    }

    async refreshNewsContent() {
        try {
            // Find news containers and refresh them
            const newsContainer = document.querySelector('.news-list, .news-grid, .table tbody');
            
            if (newsContainer) {
                // Add loading indicator
                const loadingIndicator = document.createElement('div');
                loadingIndicator.className = 'text-center p-3';
                loadingIndicator.innerHTML = '<i class="bi bi-arrow-clockwise spin"></i> Updating...';
                
                // Show loading
                newsContainer.style.opacity = '0.7';
                newsContainer.appendChild(loadingIndicator);

                // Reload the current page content via AJAX
                const response = await fetch(window.location.href, {
                    headers: {
                        'X-Requested-With': 'XMLHttpRequest'
                    }
                });

                if (response.ok) {
                    const html = await response.text();
                    const parser = new DOMParser();
                    const doc = parser.parseFromString(html, 'text/html');
                    const newContent = doc.querySelector('.news-list, .news-grid, .table tbody');
                    
                    if (newContent) {
                        newsContainer.innerHTML = newContent.innerHTML;
                    }
                }

                // Remove loading state
                newsContainer.style.opacity = '1';
                
            } else {
                // If no specific container found, reload the page
                window.location.reload();
            }
        } catch (error) {
            console.error('Error refreshing news content:', error);
            // Fallback to page reload
            window.location.reload();
        }
    }

    // Method to manually trigger refresh
    manualRefresh() {
        this.refreshNewsList();
    }

    // Get connection status
    getConnectionStatus() {
        return this.isConnected;
    }

    // Send test message
    async sendTestMessage(message) {
        if (this.connection && this.isConnected) {
            try {
                await this.connection.invoke("SendTestMessage", message);
            } catch (err) {
                console.error("Error sending test message:", err);
            }
        }
    }
}

// Initialize SignalR when page loads
document.addEventListener('DOMContentLoaded', function() {
    // Only initialize on pages that need real-time updates
    const currentPath = window.location.pathname.toLowerCase();
    
    if (currentPath.includes('/news') || 
        currentPath.includes('/staff') || 
        currentPath.includes('/admin')) {
        
        window.newsSignalR = new NewsSignalR();
        
        // Add refresh button to pages
        addRefreshButton();
    }
});

function addRefreshButton() {
    // Add a manual refresh button for real-time updates
    const refreshBtn = document.createElement('button');
    refreshBtn.className = 'btn btn-outline-secondary btn-sm position-fixed';
    refreshBtn.style.cssText = 'bottom: 20px; right: 20px; z-index: 9998;';
    refreshBtn.innerHTML = '<i class="bi bi-arrow-clockwise"></i>';
    refreshBtn.title = 'Refresh News';

    refreshBtn.addEventListener('click', function() {
        if (window.newsSignalR) {
            window.newsSignalR.manualRefresh();
        }
    });

    // Add test button for SignalR
    const testBtn = document.createElement('button');
    testBtn.className = 'btn btn-outline-info btn-sm position-fixed';
    testBtn.style.cssText = 'bottom: 60px; right: 20px; z-index: 9998;';
    testBtn.innerHTML = '<i class="bi bi-broadcast"></i>';
    testBtn.title = 'Test SignalR';

    testBtn.addEventListener('click', function() {
        if (window.newsSignalR) {
            window.newsSignalR.sendTestMessage('Test message from client');
        }
    });

    document.body.appendChild(refreshBtn);
    document.body.appendChild(testBtn);
}

// Add CSS for spinning animation
const style = document.createElement('style');
style.textContent = `
    .spin {
        animation: spin 1s linear infinite;
    }
    
    @keyframes spin {
        from { transform: rotate(0deg); }
        to { transform: rotate(360deg); }
    }
`;
document.head.appendChild(style);
