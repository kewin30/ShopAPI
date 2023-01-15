# ShopAPI
ShopAPI
This is my first API that I created on my own. I had a request with a friend to make a shop for some guys, but they ran away before it was finished.
So I kept working on the project and made some improvements. With this project, you can register and login users, make orders with products, and there is also an 
admin panel that allows the admin to create new products and easily check the status of orders.
#
I used: Automapper, FluentValidation, JwtBearer, EntityFrameworkCore, NLog.Web, Swashbuckle
So that i can easily convert one dto to another, Validate my fields, add authorization that knows who's logged in, connect it to the database and save every error in 
logs and afterall create page with every api request I created.
#
The biggest difficulty while working on this project was figuring out who was making the orders. 
I had to use claims and take it from the login and then store it. I also created integration tests for it. 
The biggest problem was with the OrderControllerTests. If I delete the last public method "AH" then 2 tests will fail, but if this name is there, then 
every test works fine. I still don't know why this happens.
![Shop](https://user-images.githubusercontent.com/83167847/212559043-2f5dfab2-28da-414e-8496-39144ac14b22.png)
![swagger1](https://user-images.githubusercontent.com/83167847/212559045-1c881177-1306-4f79-ab98-983c0e4c7eee.png)
![tests](https://user-images.githubusercontent.com/83167847/212559046-432dc527-d2d9-41bf-b367-4ef7e7f16300.png)
