# This is a sample Python script.

# Press Shift+F10 to execute it or replace it with your code.
# Press Double Shift to search everywhere for classes, files, tool windows, actions, and settings.
import pickle

from selenium import webdriver
from selenium.webdriver.support import expected_conditions as EC
from selenium.webdriver.common.by import By
from selenium.webdriver.support.wait import WebDriverWait
import sys
from sys import exit
import json

def auth(userName, passwordValue):
    # target username
    username = WebDriverWait(driver, 10).until(EC.visibility_of_element_located((By.CSS_SELECTOR, "input[name='username']")))
    password = WebDriverWait(driver, 10).until(EC.visibility_of_element_located((By.CSS_SELECTOR, "input[name='password']")))

    #enter username and password
    username.clear()
    username.send_keys(userName)
    password.clear()
    password.send_keys(passwordValue)

    #target the login button and click it
    button = WebDriverWait(driver, 2).until(EC.visibility_of_element_located((By.CSS_SELECTOR, "button[type='submit']"))).click()
    return button

result = {
    'result': '',
    'hasError': '',
    'error': ''
}

try:
    userData = sys.argv[1]

    chrome_options = webdriver.ChromeOptions()
    chrome_options.add_argument("--user-data-dir=C:/User_Data/" + userData)
    chrome_options.add_argument("--profile-directory=Default")
    chrome_options.binary_location = "C:\\inetpub\\wwwroot\\Scripts\\Chrome-bin\\chrome.exe"

    driver = webdriver.Chrome(executable_path="chrome_driver\\chromedriver.exe", options=chrome_options)

    login = sys.argv[1]
    password = sys.argv[2]

    driver.get("http://www.instagram.com")
    driver.delete_all_cookies()
    driver.refresh()

    auth(login, password)
    alert = WebDriverWait(driver, 5).until(EC.visibility_of_element_located((By.XPATH, '//button[contains(text(), "Save Info")]')))

    result = {
        'result': {
            'id': driver.session_id,
            'url': driver.command_executor._url,
            'accountId': login
        },
        'hasError': 'false'
    }
    driver.quit()
except Exception as e:
    result = {
        'hasError': 'true',
        'error': str(e)
    }

resultJson = json.dumps(result)
print(resultJson)
sys.stdout.flush()
exit(0)