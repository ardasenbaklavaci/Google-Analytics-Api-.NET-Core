First of all, create your google analytics and cloud project at Tutorial Here:

https://bogdanbujdea.dev/retrieving-data-from-google-analytics-using-net

After getting your private key json, fill it in key.json at project main directory.

Find and replace your private readonly string _propertyId = "properties/your_property_id"; 

// Replace with your Google Analytics View (Profile) ID, keep "properties/"

Now program will print the response json in output.txt, created at your main project directory...
