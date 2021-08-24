# This is a sample Python script.

# Press Shift+F10 to execute it or replace it with your code.
# Press Double Shift to search everywhere for classes, files, tool windows, actions, and settings.
import json
import pickle

from selenium import webdriver
from selenium.webdriver.common.keys import Keys
from selenium.webdriver.support import expected_conditions as EC
from selenium.webdriver.common.by import By
from selenium.webdriver.support.wait import WebDriverWait
import time
import sys
import re

driver = "NULL"

try:
    userData = sys.argv[1]

    chrome_options = webdriver.ChromeOptions()
    chrome_options.add_argument("--user-data-dir=C:/User_Data/" + userData)
    chrome_options.add_argument("--profile-directory=Default")
    chrome_options.binary_location = "C:\\inetpub\\wwwroot\\Scripts\\Chrome-bin\\chrome.exe"

    driver = webdriver.Chrome(executable_path="chrome_driver\\chromedriver.exe", options=chrome_options)

    driver.get("http://www.instagram.com/")
    alert = WebDriverWait(driver, 5).until(EC.visibility_of_element_located((By.XPATH, '//a[@href="/direct/inbox/"]')))

    result = {
        'result': {
            'success': 'true'
        },
        'hasError': 'false'
    }
except Exception as e:
    result = {
        'hasError': 'true',
        'error': str(e)
    }

driver.quit()
print(json.dumps(result))
sys.stdout.flush()