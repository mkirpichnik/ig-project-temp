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

def put_text_in_search(text):
    searchbox = WebDriverWait(driver, 10).until(EC.element_to_be_clickable((By.XPATH, "//input[@placeholder='Search']")))
    searchbox.clear()
    searchbox.send_keys(text)

def load_cookie(driver, data):
    data = data.replace('_quotes_', '"')
    for cookie in json.loads(data):
        driver.add_cookie(cookie)


def load_cookieTest(driver, path):
    with open(path, 'rb') as cookiesfile:
        cookies = pickle.load(cookiesfile)
        for cookie in cookies:
            driver.add_cookie(cookie)

result = []
driver = webdriver.Chrome("chrome_driver\\chromedriver.exe")

try:
    driver.get("http://www.instagram.com")
    load_cookie(driver, sys.argv[1])

    driver.get("http://www.instagram.com")
    time.sleep(5)

    try:
        alert = WebDriverWait(driver, 5).until(
            EC.element_to_be_clickable((By.XPATH, '//button[contains(text(), "Not Now")]'))).click()
        alert = WebDriverWait(driver, 5).until(
            EC.element_to_be_clickable((By.XPATH, '//button[contains(text(), "Not Now")]'))).click()
    except:
        time.sleep(0)

    text = sys.argv[2]
    put_text_in_search(text)
    time.sleep(5)

    anchors = driver.find_elements_by_xpath("//a[contains(@href, '/" + text + "')]")
    anchors = [a.get_attribute('href') for a in anchors]
    anchors = [a for a in anchors if re.match('https://www.instagram.com/\w*' + text + '\w*/', a)]
    result = [{ 'link':  a} for a in anchors]
except Exception as e:
    print(e)
finally:
    driver.close()
    driver.quit()

print (json.dumps(result))
sys.stdout.flush()