from locust import FastHttpUser, task
from locust.user.wait_time import constant

class WebsiteUser(FastHttpUser):
    """
    User class that does requests to the locust web server running on localhost,
    using the fast HTTP client
    """

    host = "http://127.0.0.1:8089"
    # some things you can configure on FastHttpUser
    # connection_timeout = 60.0
    # insecure = True
    # max_redirects = 5
    # max_retries = 1
    # network_timeout = 60.0
    # proxy_host = my-proxy.com
    # proxy_port = 8080
    
    wait_time = constant(30) # 30 seconds between each task

    @task
    def index(self):
        self.client.get("/")

    #@task
    #def stats(self):
    #    self.client.get("/stats/requests")
