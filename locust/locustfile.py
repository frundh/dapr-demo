from locust import FastHttpUser, task
from locust.user.wait_time import constant

class WebsiteUser(FastHttpUser):
    """
    A user class that hits the dotnetapp service
    """

    host = "http://dotnetapp:8080"
    # some things you can configure on FastHttpUser
    # connection_timeout = 60.0
    # insecure = True
    # max_redirects = 5
    # max_retries = 1
    # network_timeout = 60.0
    # proxy_host = my-proxy.com
    # proxy_port = 8080
    
    # wait_time = constant(30) # 30 seconds between each task

    @task
    def index(self):
        self.client.post("/State?updates=10&delete=true&delay=50")

    #@task
    #def stats(self):
    #    self.client.get("/stats/requests")
