using System;
using Microsoft.EntityFrameworkCore;

namespace EvergreenAPI.Models
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        public DbSet<Thumbnail> Thumbnails { get; set; }
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Blog> Blogs { get; set; }
        public DbSet<DiseaseCategory> DiseaseCategories { get; set; }
        public DbSet<Disease> Diseases { get; set; }
        public DbSet<MedicineCategory> MedicineCategories { get; set; }
        public DbSet<Medicine> Medicines { get; set; }
        public DbSet<Treatment> Treatments { get; set; }
        public DbSet<DetectionHistory> DetectionHistories { get; set; }
        public DbSet<DetectionAccuracy> DetectionAccuracies { get; set; }
        public DbSet<Image> Images{ get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            #region Thumbnail seed
            modelBuilder.Entity<Thumbnail>().HasData(
                new Thumbnail { ThumbnailId = 1, Url = "https://www.fao.org.vn/wp-content/uploads/2019/08/benh-vang-la-greening.jpg", AltText = "Hình ảnh bệnh vàng lá" },
                new Thumbnail { ThumbnailId = 2, Url = "https://i0.wp.com/trongraulamvuon.com/wp-content/uploads/2013/11/sau-duc-than-hong.jpg", AltText = "Hình ảnh bệnh sâu đục thân" },
                new Thumbnail { ThumbnailId = 3, Url = "https://hoadepviet.com/wp-content/uploads/2016/11/benh-hoa-hong-1.jpg", AltText = "Hình ảnh bệnh lá úa sớm" },
                new Thumbnail { ThumbnailId = 4, Url = "https://www.wondriumdaily.com/wp-content/uploads/2017/07/thumbnail-7.jpg", AltText = "Tomato thumbnail 1" },
                new Thumbnail { ThumbnailId = 5, Url = "https://www.skh.com/wp-content/uploads/2021/04/FF-peppers-tomatoes-onions-thumbnail.jpg", AltText = "Tomato thumbnail 2" },
                new Thumbnail { ThumbnailId = 6, Url = "https://sp.apolloboxassets.com/vendor/product/productImages/2022-09-15/6JBvcArray_13.jpg", AltText = "Tomato thumbnail 3" },
                new Thumbnail { ThumbnailId = 7, Url = "https://api-static.bacsicayxanh.vn/pictures/0001571_chaetomium_500.jpeg", AltText = "Chaetomium" },
                new Thumbnail { ThumbnailId = 8, Url = "https://www.basudin.com/storage/settings/May2021/basudin.png", AltText = "Basudin" },
                new Thumbnail { ThumbnailId = 9, Url = "https://phanthuocvisinh.com/wp-content/uploads/2021/12/AT-Vaccino-CAN-500ml.jpg", AltText = "AT Vaccino" },
                new Thumbnail { ThumbnailId = 10, Url = "https://mygarden.vn/wp-content/uploads/2020/11/tri-sau-an-la-2.jpg", AltText = "Bắt sâu bệnh" },
                new Thumbnail { ThumbnailId = 11, Url = "https://xuannong.vn/images/bo-tri-tren-cay-mai.jpg", AltText = "Lặt bỏ lá bị nhiễm bệnh" },
                new Thumbnail { ThumbnailId = 12, Url = "https://trongrauthuycanhtphcm.files.wordpress.com/2018/05/aphidadults-and-nymphs.jpg", AltText = "Lau cây bằng nước xà phòng hoặc rượu." },
                new Thumbnail { ThumbnailId = 13, Url = "https://baotayninh.vn/image/fckeditor/upload/2017/20170808/images/tieu%20huy_TP.JPG", AltText = "Tiêu hủy các cây bị nhiễm bệnh" },
                new Thumbnail { ThumbnailId = 14, Url = "http://www.congtyhai.com/Data/Sites/1/News/1348/hopsan-75ec_250ml_1618299173.png", AltText = "Hopsan 75EC" },
                new Thumbnail { ThumbnailId = 15, Url = "https://congnghesinhhocwao.vn/wp-content/uploads/2022/06/Xu-ly-triet-de-nguyen-nhan-gaybenh-7-1.jpg", AltText = "Cách phòng ngừa bệnh vàng lá" },
                new Thumbnail { ThumbnailId = 16, Url = "https://cdn.tgdd.vn/Files/2017/10/30/1037058/9-cong-dung-va-han-che-cua-ca-chua-doi-voi-cuoc-song-hang-ngay-202103142026330054.jpg", AltText = "Nâng cao năng suất cà chua" },
                new Thumbnail { ThumbnailId = 17, Url = "https://suckhoedoisong.qltns.mediacdn.vn/324455921873985536/2021/9/25/tac-dung-cua-ca-chua-doi-voi-suc-khoe-1-1632310636-831-width640height427-1632567723926-16325677242441321628137.jpg", AltText = "Cà chua và sức khoẻ" }
            );
            #endregion

            #region Blog seed

            modelBuilder.Entity<Blog>().HasData(
                new Blog
                {
                    BlogId = 1,
                    Title = "The Harmful Effects of Aphids",
                    Description = "Aphids are small, soft-bodied insects that are commonly found on plants. While they may seem harmless, these insects can have significant negative effects on plant growth and health. In this article, we'll explore the harmful effects of aphids and how they can be controlled. ",
                    Content = "<div class=\"markdown prose w-full break-words dark:prose-invert light\"><h2>Plant Damage</h2><p>One of the most obvious effects of aphids on plants is the physical damage they can cause. Aphids feed on the sap of plants, which can weaken and deform the leaves, stems, and flowers. This can lead to stunted growth, reduced yield, and even death in severe cases.</p><p>In addition to the physical damage, aphids can also transmit plant diseases. Some species of aphids are carriers of plant viruses, which can cause a range of symptoms such as yellowing, wilting, and spotting on the leaves. These diseases can have devastating effects on crops and can lead to significant economic losses.</p><h2>Honeydew Production</h2><p>Another harmful effect of aphids is their production of honeydew. Honeydew is a sticky substance that aphids excrete as they feed on plant sap. This substance can attract other insects such as ants and wasps, which can disrupt the natural balance of the ecosystem. Honeydew can also create a favorable environment for the growth of sooty mold, a black fungus that can further damage the plant.</p><h2>Reduction in Plant Vigor</h2><p>Aphids can also reduce the overall vigor of plants. When a plant is infested with aphids, it has to use more energy to defend itself against the insects. This can lead to a reduction in the plant's ability to grow and produce fruit. In addition, the stress caused by the aphid infestation can make the plant more vulnerable to other environmental stresses such as drought and extreme temperatures.</p><h2>Control Measures</h2><p>There are several control measures that can be used to manage aphid infestations. The most effective method is to use natural predators such as ladybugs and lacewings. These insects feed on aphids and can significantly reduce the population of the pests.</p><p>Chemical pesticides can also be used to control aphids, but these should be used with caution as they can harm beneficial insects and pollinators. It is important to choose a pesticide that is specifically designed for aphids and to follow the manufacturer's instructions carefully.</p><p>Another method of control is to remove the aphids manually. This can be done by spraying the plant with a strong jet of water or by using a sticky trap. In severe cases, it may be necessary to prune and discard heavily infested parts of the plant.</p><h2>Conclusion</h2><p>Aphids may seem like harmless pests, but they can have significant negative effects on plant growth and health. By understanding the harmful effects of aphids, it is possible to take measures to control their population and minimize the damage they cause. Whether through natural predators, chemical pesticides, or manual removal, it is important to act quickly to prevent an infestation from becoming a serious problem.</p></div>",
                    ViewCount = 10,
                    ThumbnailId = 4
                    
                },
                new Blog
                {
                    BlogId = 2,
                    Title = "The Many Uses of Tomatoes",
                    Description = "Tomatoes are a versatile fruit that can be used in a variety of ways. They are low in calories, high in vitamins, and contain antioxidants. Here are just a few of the many uses of tomatoes:",
                    Content = "<div class=\"markdown prose w-full break-words dark:prose-invert light\"><h2>Culinary Uses</h2><ol><li><p>Sauce - Tomatoes are the main ingredient in many popular sauces, such as marinara, salsa, and ketchup.</p></li><li><p>Soup - Tomato soup is a classic comfort food that can be served hot or cold.</p></li><li><p>Salad - Tomatoes can be sliced or diced and added to salads for a fresh burst of flavor.</p></li><li><p>Sandwich - Tomatoes are a common ingredient in sandwiches, adding moisture and texture.</p></li><li><p>Pizza - Pizza sauce is typically made with tomato sauce, and fresh tomatoes can also be added as a topping.</p></li><li><p>Juice - Tomato juice is a popular beverage that is high in vitamins and minerals.</p></li><li><p>Stuffed - Tomatoes can be hollowed out and stuffed with a variety of fillings, such as rice, cheese, or ground meat.</p></li><li><p>Roasted - Roasted tomatoes have a sweet and savory flavor that can be used in a variety of dishes.</p></li></ol><h2>Health and Beauty Uses</h2><ol><li><p>Skincare - Tomatoes are high in antioxidants, which can help to reduce inflammation and prevent aging. They can be used in homemade face masks and scrubs.</p></li><li><p>Sunburn Relief - The acidity in tomatoes can help to soothe sunburned skin.</p></li><li><p>Haircare - Tomatoes contain vitamins and minerals that can help to promote healthy hair growth. They can be used in hair masks and rinses.</p></li><li><p>Weight Loss - Tomatoes are low in calories and high in fiber, making them a great addition to a weight loss diet.</p></li></ol><h2>Household Uses</h2><ol><li><p>Stain Remover - Tomatoes can be used to remove stains from clothing and carpets.</p></li><li><p>Garden Fertilizer - Tomatoes are high in nutrients that are beneficial to plants. They can be used as a natural fertilizer in the garden.</p></li><li><p>Insect Repellent - The scent of tomatoes can repel certain insects, such as aphids and whiteflies.</p></li><li><p>Polisher - Tomatoes can be used to polish copper and brass items.</p></li></ol><p>In conclusion, tomatoes are a versatile fruit that can be used in many different ways. From culinary uses to health and beauty applications, and even household uses, the tomato is a valuable and indispensable ingredient in our daily lives. So next time you're looking for a tasty and nutritious addition to your meal or a natural solution for a household problem, consider reaching for a tomato.</p></div>",
                    LastModifiedDate = DateTime.Today - TimeSpan.FromDays(2),
                    ViewCount = 3,
                    ThumbnailId = 5
                    
                },
                new Blog
                {
                    BlogId = 3,
                    Title = "Cultivating Land for Crops: A Guide to Getting Started",
                    Description = "Cultivating land for crops is a process that involves preparing the soil, planting seeds, and managing the growth of the plants. This is a crucial step in agriculture, as it is the foundation for producing healthy and abundant crops. Here are some key considerations for cultivating land for crops:",
                    Content = "<div class=\"markdown prose w-full break-words dark:prose-invert light\"><h2>Planting Seeds</h2><p>After the soil has been prepared, it's time to plant the seeds. The type of seeds that are planted will depend on the crop being grown and the climate in the region. Some crops, such as corn and soybeans, are commonly grown in many regions, while others, such as tropical fruits, are only grown in specific regions.</p><p>When planting seeds, it's important to follow the recommended spacing and depth guidelines for each crop. Seeds should be planted at a depth that allows for good soil contact and adequate moisture. After planting, the soil should be gently compacted to ensure good seed-to-soil contact.</p><h2>Managing Growth</h2><p>Once the seeds have been planted, it's important to manage the growth of the plants. This involves monitoring the plants for signs of pests, diseases, and nutrient deficiencies. Regular irrigation and fertilization can also help to ensure healthy plant growth.</p><p>Weed control is another important aspect of managing the growth of crops. Weeds can compete with crops for nutrients and water, which can reduce crop yields. Cultivators can use a variety of methods to control weeds, including hand weeding, hoeing, and the use of herbicides.</p><h2>Harvesting</h2><p>The final step in cultivating land for crops is harvesting the crops. This involves carefully removing the crops from the soil and preparing them for sale or storage. Some crops, such as fruits and vegetables, are hand-picked, while others, such as grains and beans, are harvested using machinery.</p><p>Proper storage is also important for preserving the quality and freshness of harvested crops. Many crops need to be stored in cool, dry conditions to prevent spoilage and rot.</p><p>In conclusion, cultivating land for crops is a complex process that involves careful preparation, planting, and management. By following these steps, farmers and cultivators can produce healthy and abundant crops that contribute to food security and economic development.</p></div>",
                    LastModifiedDate = DateTime.Today - TimeSpan.FromDays(10),
                    ViewCount = 30,
                    ThumbnailId = 6
                },
                new Blog
                {
                    BlogId = 4,
                    Title = "How to Prevent Tomato Yellow Leaf Disease",
                    Description = "Tomato yellow leaf disease is a common problem that affects tomato plants, causing leaves to turn yellow and wilt. This disease is caused by a virus and can be spread by insects, such as whiteflies. Here are some steps you can take to prevent tomato yellow leaf disease:",
                    Content = "<div class=\"markdown prose w-full break-words dark:prose-invert light\"><h2>Soil Preparation</h2><p>The first step in cultivating land for crops is preparing the soil. This involves removing any debris or vegetation from the surface of the soil and tilling the soil to loosen it up. Tilling helps to aerate the soil, which allows oxygen and water to penetrate deeper into the ground.</p><p>Once the soil has been tilled, it's important to test the soil's pH levels and nutrient content. Soil testing can help to determine the best type of fertilizer to use and how much to apply. Adding compost or other organic matter to the soil can also help to improve its structure and fertility.</p><h2>Planting Seeds</h2><p>After the soil has been prepared, it's time to plant the seeds. The type of seeds that are planted will depend on the crop being grown and the climate in the region. Some crops, such as corn and soybeans, are commonly grown in many regions, while others, such as tropical fruits, are only grown in specific regions.</p><p>When planting seeds, it's important to follow the recommended spacing and depth guidelines for each crop. Seeds should be planted at a depth that allows for good soil contact and adequate moisture. After planting, the soil should be gently compacted to ensure good seed-to-soil contact.</p><h2>Managing Growth</h2><p>Once the seeds have been planted, it's important to manage the growth of the plants. This involves monitoring the plants for signs of pests, diseases, and nutrient deficiencies. Regular irrigation and fertilization can also help to ensure healthy plant growth.</p><p>Weed control is another important aspect of managing the growth of crops. Weeds can compete with crops for nutrients and water, which can reduce crop yields. Cultivators can use a variety of methods to control weeds, including hand weeding, hoeing, and the use of herbicides.</p><h2>Harvesting</h2><p>The final step in cultivating land for crops is harvesting the crops. This involves carefully removing the crops from the soil and preparing them for sale or storage. Some crops, such as fruits and vegetables, are hand-picked, while others, such as grains and beans, are harvested using machinery.</p><p>Proper storage is also important for preserving the quality and freshness of harvested crops. Many crops need to be stored in cool, dry conditions to prevent spoilage and rot.</p><p>In conclusion, cultivating land for crops is a complex process that involves careful preparation, planting, and management. By following these steps, farmers and cultivators can produce healthy and abundant crops that contribute to food security and economic development.</p></div>",
                    LastModifiedDate = DateTime.Today - TimeSpan.FromDays(5),
                    ViewCount = 60,
                    ThumbnailId = 15
                },
                new Blog
                {
                    BlogId = 5,
                    Title = "How to Improve Tomato Yield",
                    Description = "Tomatoes are a popular garden crop that can provide a bountiful harvest of delicious fruit. However, getting the most out of your tomato plants requires careful planning and attention to detail. Here are some tips for improving tomato yield:",
                    Content = "<div class=\"markdown prose w-full break-words dark:prose-invert light\"><h2>Avoid Overwatering</h2><p>Overwatering can make tomato plants more susceptible to disease. This is because excess moisture can create the ideal conditions for fungal and bacterial growth. To prevent tomato yellow leaf disease, it's important to water your plants carefully, making sure to only water the soil and not the leaves. Avoid watering in the evening, as this can increase the risk of fungal growth.</p><h2>Fertilize Properly</h2><p>Proper fertilization can help to prevent tomato yellow leaf disease by promoting healthy plant growth. It's important to use a balanced fertilizer that contains the right ratio of nitrogen, phosphorus, and potassium. Too much nitrogen can make plants more susceptible to disease, so it's important to use fertilizers in moderation.</p><p>In conclusion, preventing tomato yellow leaf disease requires a combination of preventative measures, including planting resistant varieties, controlling insects, maintaining good hygiene, avoiding overwatering, and fertilizing properly. By following these steps, you can help to ensure the health and productivity of your tomato plants, and enjoy a bountiful harvest of delicious tomatoes.</p></div>",
                    LastModifiedDate = DateTime.Today - TimeSpan.FromDays(55),
                    ViewCount = 144,
                    ThumbnailId = 16
                },
                new Blog
                {
                    BlogId = 6,
                    Title = "Septoria Prevention: A Guide to Protecting Your Plants",
                    Description = "Septoria is a common fungal disease that affects plants, particularly those in the tomato family. It can cause significant damage to leaves and reduce yield, making it an important consideration for any gardener or farmer. Fortunately, there are several steps you can take to prevent Septoria from taking hold in your plants. In this article, we’ll explore some of the best practices for Septoria prevention.",
                    Content = "<div class=\"markdown prose w-full break-words dark:prose-invert light\"><h2>Choose Resistant Varieties</h2><p>One of the most effective ways to prevent Septoria is to choose tomato varieties that are resistant to the disease. Look for varieties that have been bred specifically for this purpose, such as 'Iron Lady' or 'Mountain Magic.' These varieties are more resistant to the fungus and are less likely to develop the disease.</p><h2>Keep Plants Dry</h2><p>Septoria thrives in damp, humid conditions, so it’s important to keep your plants as dry as possible. Water at the base of the plant rather than from above, as this will help keep the leaves dry. If you use a sprinkler system, try to water early in the day so that the leaves have time to dry before nightfall.</p><h2>Rotate Crops</h2><p>Another way to prevent Septoria is to rotate your crops. This means planting tomatoes or other susceptible plants in a different location each year. This helps prevent the fungus from building up in the soil and reduces the likelihood of infection.</p><h2>Prune and Stake Plants</h2><p>Proper pruning and staking can also help prevent Septoria. By pruning your plants, you can remove infected leaves and improve air circulation around the plant. Staking helps keep the plants upright and allows more air to circulate around the leaves, which can help prevent moisture buildup.</p><h2>Apply Fungicides</h2><p>If all else fails, you may need to apply a fungicide to prevent or control Septoria. There are several fungicides available that are effective against the disease, including copper-based fungicides and those containing chlorothalonil. Be sure to follow the instructions carefully and use the appropriate protective gear when applying these products.</p><h2>Conclusion</h2><p>Preventing Septoria is an important consideration for any gardener or farmer who grows tomatoes or other susceptible plants. By choosing resistant varieties, keeping plants dry, rotating crops, pruning and staking, and applying fungicides as needed, you can minimize the risk of infection and protect your plants from this damaging disease. With these best practices in mind, you can enjoy healthy, productive plants throughout the growing season.</p></div>",
                    LastModifiedDate = DateTime.Today - TimeSpan.FromDays(1),
                    ViewCount = 120,
                    ThumbnailId = 17
                },
                new Blog
                {
                    BlogId = 7,
                    Title = "Early Blight Prevention: How to Protect Your Plants",
                    Description = "Early blight is a common disease that affects tomato plants. It is caused by a fungus called Alternaria solani and can result in significant yield losses if left untreated. The good news is that early blight can be prevented with a few simple measures. In this article, we will discuss some effective strategies for early blight prevention.",
                    Content = "<div class=\"markdown prose w-full break-words dark:prose-invert light\"><h2>Crop Rotation</h2><p>Crop rotation is one of the most effective ways to prevent early blight. This involves alternating the planting location of tomato plants with other crops such as corn, beans, or lettuce. This helps to reduce the buildup of fungal spores in the soil and prevents the disease from spreading to new plants. It is important to avoid planting tomatoes in the same spot for at least three years to allow the soil to recover.</p><h2>Proper Spacing</h2><p>Another important factor in early blight prevention is proper spacing between tomato plants. Crowded plants are more susceptible to disease because they provide an ideal environment for fungal growth. It is recommended to space tomato plants at least two feet apart and remove any suckers or lower leaves to improve air circulation.</p><h2>Mulching</h2><p>Mulching is an effective way to prevent early blight by reducing soil splash and preventing fungal spores from coming into contact with the leaves. Mulch also helps to retain soil moisture and regulate soil temperature, which can improve plant health. Organic mulch such as straw or leaves is recommended, as it provides additional nutrients to the soil as it decomposes.</p><h2>Watering</h2><p>Proper watering is essential for healthy tomato plants and can also help to prevent early blight. It is important to water plants at the base rather than from above to avoid getting water on the leaves. Wet leaves provide an ideal environment for fungal growth, so it is best to water in the morning so that the leaves can dry during the day. Avoid overwatering, as this can lead to root rot and other issues.</p><h2>Fungicides</h2><p>Fungicides can be used as a last resort to prevent early blight. There are many different types of fungicides available, including both organic and synthetic options. It is important to read the label carefully and follow the instructions for proper application. Fungicides should only be used when necessary and in conjunction with other preventative measures such as crop rotation and proper spacing.</p><p>In conclusion, early blight can be prevented by implementing a few simple measures. Crop rotation, proper spacing, mulching, proper watering, and fungicides can all help to prevent the disease from spreading to your tomato plants. By following these guidelines, you can protect your plants and ensure a healthy and abundant harvest.</p></div>",
                    LastModifiedDate = DateTime.Today - TimeSpan.FromDays(1),
                    ViewCount = 120,
                    ThumbnailId = 17
                },
                new Blog
                {
                    BlogId = 8,
                    Title = "The Harmful Effects of Eating Too Many Tomatoes",
                    Description = "Tomatoes are a nutritious and delicious fruit that are often used in a variety of dishes. However, like anything else, consuming too many tomatoes can have harmful effects on your health. In this article, we will discuss some of the negative effects of eating too many tomatoes.",
                    Content = "<div class=\"markdown prose w-full break-words dark:prose-invert light\"><h2>High Acid Content</h2><p>Tomatoes are highly acidic, which can cause problems for people who have sensitive stomachs or suffer from acid reflux. Consuming too many tomatoes can cause heartburn, indigestion, and other digestive issues. It is important to limit your tomato consumption if you experience any of these symptoms.</p><h2>Allergies</h2><p>Some people may be allergic to tomatoes, which can cause a range of symptoms including itching, hives, and swelling. In severe cases, tomato allergies can cause anaphylaxis, a life-threatening allergic reaction. If you experience any of these symptoms after consuming tomatoes, it is important to seek medical attention immediately.</p><h2>Kidney Stones</h2><p>Tomatoes contain a high level of oxalates, which can lead to the formation of kidney stones. Consuming too many tomatoes can increase the risk of developing kidney stones, especially for people who are already prone to this condition. It is important to speak with your doctor if you have a history of kidney stones and want to include tomatoes in your diet.</p><h2>Vitamin C Overload</h2><p>Tomatoes are a great source of vitamin C, but consuming too much of it can lead to vitamin C overload. This can cause diarrhea, nausea, and other digestive issues. It is important to consume vitamin C in moderation and not rely solely on tomatoes as your source of this important nutrient.</p><p>In conclusion, while tomatoes are a nutritious and delicious fruit, it is important to consume them in moderation. Consuming too many tomatoes can have harmful effects on your health, including digestive issues, allergies, kidney stones, and vitamin C overload. It is important to speak with your doctor if you have any concerns about including tomatoes in your diet.</p></div>",
                    LastModifiedDate = DateTime.Today - TimeSpan.FromDays(1),
                    ViewCount = 12,
                    ThumbnailId = 17
                },
                new Blog
                {
                    BlogId = 9,
                    Title = "Crossbreeding Tomato Plants: A Guide",
                    Description = "Tomatoes are a popular crop worldwide, and their cultivation has been extensively studied for centuries. Crossbreeding is a common method of plant breeding that involves combining desirable traits from two different plants. In this blog post, we'll explore the process of crossbreeding tomato plants and how it can be used to create new varieties with unique characteristics.",
                    Content = "<div class=\"markdown prose w-full break-words dark:prose-invert light\"><h2>The Process of Crossbreeding Tomato Plants</h2><p>Crossbreeding tomato plants involves transferring pollen from one plant to another. This is usually done manually to ensure that the right pollen is used. Here are the steps to follow:</p><h3>Step 1: Choose the Parent Plants</h3><p>To crossbreed tomato plants, you need to choose the parent plants carefully. You should choose two plants with desirable traits that you want to combine in the offspring. For example, you might choose one plant that is resistant to a certain disease and another plant that has a high yield.</p><h3>Step 2: Prepare the Parent Plants</h3><p>Before crossbreeding, you need to prepare the parent plants. This involves removing the flowers from one of the plants so that it cannot self-pollinate. This plant will become the female parent. The other plant will become the male parent and will be used to provide the pollen.</p><h3>Step 3: Transfer the Pollen</h3><p>Once the male plant has produced enough pollen, you need to transfer it to the female plant. You can do this by removing the flower from the male plant and brushing the pollen onto the stigma of the female flower. You can also use a small brush or cotton swab to transfer the pollen.</p><h3>Step 4: Wait for the Fruit to Develop</h3><p>After pollination, you need to wait for the fruit to develop. The resulting fruit will be a hybrid of the two parent plants and will have some of the traits of each parent. You can then collect the seeds from the fruit and grow them to produce the next generation of plants.</p><h2>Tips for Successful Crossbreeding</h2><p>Crossbreeding tomato plants can be a challenging process, but there are some tips that can help you succeed:</p><ul><li>Choose parent plants with complementary traits that you want to combine.</li><li>Ensure that the parent plants are healthy and free from disease.</li><li>Use a small brush or cotton swab to transfer the pollen to avoid damaging the flowers.</li><li>Label the hybrid fruit and keep track of the parent plants used.</li><li>Collect the seeds from the hybrid fruit and plant them to grow the next generation of plants.</li></ul><h2>Conclusion</h2><p>Crossbreeding tomato plants can be a fun and rewarding process that allows you to create new varieties with unique characteristics. By following the steps outlined in this guide and using the tips for success, you can create your own tomato varieties and contribute to the diversity of the tomato plant population. Happy crossbreeding!</p></div>",
                    LastModifiedDate = DateTime.Today - TimeSpan.FromDays(1),
                    ViewCount = 120,
                    ThumbnailId = 17
                });

            #endregion

            #region DiseaseCategory seed
            modelBuilder.Entity<DiseaseCategory>().HasData(
                new DiseaseCategory
                {
                    DiseaseCategoryId = 1,
                    Name = "Diseases Caused By Weather"
                },
                new DiseaseCategory
                {
                    DiseaseCategoryId = 2,
                    Name = "Diseases Caused By Pests"
                },
                new DiseaseCategory
                {
                    DiseaseCategoryId = 3,
                    Name = "Diseases Caused By Viruses"
                },
                new DiseaseCategory
                {
                    DiseaseCategoryId = 4,
                    Name = "Healthy Leaf"
                }

                );
            #endregion


            #region Disease seed

            modelBuilder.Entity<Disease>().HasData(
                new Disease
                {
                    DiseaseId = 1,
                    Name = "Early Blight",
                    Identification = "Early blight is identified by the appearance of a few (5 to 10 in most cases) circular brown spots on a leaf. " +
                    "The spots are up to a half-inch in diameter, with concentric rings or ridges that form a target-like pattern surrounded by a yellow halo." +
                    " As the disease progresses, spots merge together and may kill the whole leaf. Over time, the stem and fruit may also be infected, showing dark and sunken spots." +
                    " Cankers with a similar dark and sunken target-like appearance are often found at or above the soil line on the stem",
                    
                    
                    Affect = "Under favorable conditions (e.g., warm weather with short or abundant dews), significant defoliation of lower leaves may occur, leading to sunscald of the fruit." +
                    " As the disease progresses, symptoms may migrate to the plant stem and fruit. Stem lesions are dark, slightly sunken and concentric in shape. " +
                    "Seedlings can develop small, dark, partially sunken lesions which grow and elongate into circular or oblong lesions. " +
                    "Basal girdling and death of seedlings may occur, a symptom known as collar rot. " +
                    "It brings significant damage to tomato leaves, stems, and fruits almost yearly in West Virginia. " +
                    "Early blight can also affect potato foliage.",
                    DiseaseCategoryId = 3,
                    ThumbnailId = 1,
                    MedicineId = 1,
                    TreatmentId = 1
                },
                new Disease
                {
                    DiseaseId = 2,
                    Name = "Septoria",
                    Identification = "The first symptoms appear as small, water-soaked, circular spots 1/16 to 1/8\" in diameter on the undersides of older leaves. " +
                    "The centers of these spots then turn gray to tan and have a dark-brown margin. The spots are distinctively circular and are often quite numerous. " +
                    "As the spots age, they sometimes enlarge and often coalesce. A diagnostic feature of this disease is the presence of many dark-brown, pimple-like structures called pycnidia (fruiting bodies of the fungus) that are readily visible in the tan centers of the spots. When spots are numerous, affected leaves turn yellow and eventually shrivel up, brown, and drop off. " +
                    "Defoliation usually starts on the oldest leaves and can quickly spread progressively up the plant toward the new growth. Significant losses can result from early leaf-drop and often leads to the subsequent sunscalding of the fruit when plants are prematurely defoliated.",
                    
                    
                    Affect = "If untreated, Septoria leaf spot will cause the leaves to turn yellow and eventually to dry out and fall off. " +
                    "This will weaken the plant, send it into decline, and cause sun scalding of the unprotected, exposed tomatoes. " +
                    "Without leaves, the plant will not continue producing and maturing tomatoes. " +
                    "Septoria leaf spot spreads rapidly.",
                    DiseaseCategoryId = 2,
                    ThumbnailId = 2,
                    MedicineId = 2,
                    TreatmentId = 2
                },
                new Disease
                {
                    DiseaseId = 3,
                    Name = "Yellow Curl",
                    Identification = "The most obvious symptoms in tomato plants are small leaves that become yellow between the veins. " +
                    "The leaves also curl upwards and towards the middle of the leaf. Plants are stunted or dwarfed. New growth only produced after infection is reduced in size. Leaflets are rolled upwards and inwards. " +
                    "Leaves are often bent downwards, stiff, thicker than normal, have a leathery texture, show interveinal chlorosis and are wrinkled. " +
                    "Young leaves are slightly chlorotic (yellowish)",

                    Affect = "Tomato yellow leaf curl can infect over 30 different kinds of plants, but it is mainly known to cause devastating losses of up to 100 per cent in the yield of tomatoes. Both field and glasshouse grown tomatoes are susceptible." +
                    "Tomato yellow leaf curl can infect over 30 different kinds of plants, but it is mainly known to cause devastating losses of up to 100 per cent in the yield of tomatoes. " +
                    "Both field and glasshouse grown tomatoes are susceptible. In seedlings, the shoots become shortened and give the young plants a bushy appearance. In mature plants only new growths produced after infection is reduced in size.",
                    DiseaseCategoryId = 1,
                    ThumbnailId = 3,
                    MedicineId = 3,
                    TreatmentId = 3
                },
                new Disease
                {
                    DiseaseId = 4,
                    Name = "Healthy Leaf",
                    Identification = "Leaves are green evenly, not bent down, no ring spots, yellow spots appear. Notice that the leaves on the tomato plant are 10 inches long on an average stem." +
                    "Small leaflets are three inches long. " +
                    "Look at the leaves and notice the serrated, or wavy and pointed, edging along the entire leaf.",
                    DiseaseCategoryId = 4,
                    ThumbnailId = 4,
                    MedicineId = 5,
                    TreatmentId = 5
                },
                new Disease
                {
                    DiseaseId = 5,
                    Name = "Aphid Bugs",

                    Identification = "Aphids are tiny, soft-bodied insects usually found on the underside of leaves and feeding on new, soft growth. " +
                    "They can be green, pink, purple, gray, or black in color. When squished, the bodies usually release a similarly colored pigment. " +
                    "Aphids are usually found in colonies producing honeydew.",

                    Affect = "Aphids remove sap from the plant with their piercing-sucking mouthparts. Tomato plants can tolerate large numbers of aphids without suffering yield loss. " +
                    "However, severe infestations can cause leaves to curl and may stunt plants. " +
                    "Decreased leaf area can increase sun scald to the fruit.",
                    DiseaseCategoryId = 2,
                    ThumbnailId = 5,
                    MedicineId = 4,
                    TreatmentId = 4
                },
                new Disease
                {
                    DiseaseId = 6,
                    Name = "Tomato Mosaic Virus",

                    Identification = "Tomato mosaic virus (ToMV) can be identified through a combination of symptoms, diagnostic tests, and molecular techniques. Symptoms of ToMV infection include yellowing and mottling of leaves, stunted growth, and reduced fruit production." +
                    "erological assays, such as enzyme-linked immunosorbent assay (ELISA), can be used to detect the presence of ToMV in infected plant tissues. Nucleic acid-based tests, such as reverse transcription polymerase chain reaction (RT-PCR) and real-time PCR, can detect the virus directly by amplifying its genetic material." +
                    "Electron microscopy can be used to visualize the virus particles directly. ",

                    Affect = "Tomato mosaic virus (ToMV) can cause a range of symptoms in infected tomato plants. The most common symptom of ToMV infection is the appearance of yellow or light-green mottling on the leaves. The leaves may also become distorted and curl upwards.  Infected plants may be stunted in growth and produce fewer flowers and fruits than healthy plants." + "Infected fruits may be smaller, misshapen, and have a reduced flavor compared to healthy fruits. Infected plants may produce fewer fruits than healthy plants, leading to a reduced overall yield." + "",
                    DiseaseCategoryId = 3,
                    ThumbnailId = 5,
                    MedicineId = 4,
                    TreatmentId = 4
                },
                new Disease
                {
                    DiseaseId = 7,
                    Name = "Bacterial spot",

                    Identification = "Bacterial spot of tomato is a plant disease caused by the bacterium Xanthomonas campestris pv. vesicatoria. The disease affects the leaves, stems, and fruit of tomato plants, causing circular or irregular spots on the foliage." +
                    "The disease first appears as small, water-soaked lesions on the leaves that gradually enlarge to become circular or irregular in shape. The spots are typically dark brown to black and may have a yellow halo around them. In severe cases, bacterial spot can cause lesions on the stems of tomato plants, which can lead to wilting and death of the plant." +
                    "The disease can also cause lesions on the fruit, which appear as small, water-soaked spots that eventually turn brown and sunken. The lesions caused by bacterial spot of tomato often have a raised center, giving them a characteristic bull's-eye appearance." +
                    "Unlike many other leaf spot diseases, bacterial spot lesions often have a distinctive angular shape. If you cut into a lesion on a tomato plant infected with bacterial spot, you may see a yellowish-brown bacterial ooze. ",

                    Affect = "Bacterial spot of tomato can have significant negative effects on the health and productivity of tomato plants. Bacterial spot can cause significant damage to tomato plants, reducing the overall yield of the crop. The disease can cause defoliation of tomato plants, which can impact their ability to photosynthesize and produce energy." +
                    "In severe cases, bacterial spot can cause wilting of tomato plants, which can lead to their death. The disease can cause lesions on the fruit of tomato plants, making them unsightly and reducing their market value. " +
                    "Bacterial spot can easily spread from plant to plant, and can infect other members of the nightshade family, such as peppers and eggplants. Bacterial spot can cause significant economic losses for tomato growers, as infected crops may be unsalable or have reduced value.",
                    DiseaseCategoryId = 3,
                    ThumbnailId = 5,
                    MedicineId = 4,
                    TreatmentId = 4
                },
                new Disease
                {
                    DiseaseId = 8,
                    Name = "Spider mites Two spotted spider mite",

                    Identification = "Two-spotted spider mites are tiny arachnids that can cause damage to tomato plants. Two-spotted spider mites are very small, measuring about 0.5 mm in length. They are barely visible to the naked eye. The mites have a yellowish-green body with two dark spots on their back. Two-spotted spider mites spin fine webs on the undersides of leaves, which can be a telltale sign of their presence." +
                    "The mites feed on the sap of the leaves, causing yellowing and browning of the foliage. In severe infestations, leaves may become completely brown and fall off the plant. Two-spotted spider mites lay small, round eggs on the undersides of leaves, which can be seen with a magnifying glass. Two-spotted spider mites can reproduce quickly, with a single female laying up to 100 eggs in her lifetime.",

                    Affect = "Two-spotted spider mites can have significant negative effects on the health and productivity of tomato plants. Two-spotted spider mites can cause significant damage to tomato plants, reducing the overall yield of the crop. The mites feed on the sap of the leaves, causing yellowing and browning of the foliage. In severe infestations, leaves may become completely brown and fall off the plant, leading to defoliation and reduced photosynthesis." +
                    "Two-spotted spider mites can stunt the growth of tomato plants, leading to smaller and less productive plants. The mites can also cause damage to the fruit of tomato plants, making them unsightly and reducing their market value. Two-spotted spider mites can easily spread from plant to plant, and can infect other plants in the same area. Two-spotted spider mites can develop resistance to pesticides over time, making it more difficult to control infestations.",
                    DiseaseCategoryId = 2,
                    ThumbnailId = 5,
                    MedicineId = 4,
                    TreatmentId = 4
                }


            );

            #endregion




            #region MedicineCategory seed

            modelBuilder.Entity<MedicineCategory>().HasData(
              new MedicineCategory { MedicineCategoryId = 1, Name = "Pesticide" },
              new MedicineCategory { MedicineCategoryId = 2, Name = "Fertilizer" },
              new MedicineCategory { MedicineCategoryId = 3, Name = "Fungicide" },
              new MedicineCategory { MedicineCategoryId = 4, Name = "Healthy Leaf" }
            );

            #endregion





            #region Medicine seed

            modelBuilder.Entity<Medicine>().HasData(
                new Medicine
                {
                    MedicineId = 1,
                    Name = "Bonide Liquid Copper Fungicide Concentrate",

                    Uses = "Control common plant diseases in your lawn and home garden with Captain Jack’s Ready-to-Spray Liquid Copper Fungicide. " +
                    "Approved for organic gardening, Copper Fungicide can be used on a variety of listed fruits, vegetables, nuts, herbs and flowers, and can even be applied up to the day of harvest. " +
                    "Captain Jack’s Copper Fungicide is effective against common fungal diseases including downy mildew, black rot, leaf spot, powdery mildew, blight, peach leaf curl and more. " +
                    "This product arrives conveniently ready-to-mix. For best results, mix according to label and apply using a plant sprayer. Apply thoroughly to the tops and undersides of leaves and all plant surfaces on affected plants. " +
                    "Repeat every 7-10 days as needed. Please see product label for full use instructions.",
                    MedicineCategoryId = 2,
                    ThumbnailId = 7,
                    


                },
                new Medicine
                {
                    MedicineId = 2,
                    Name = "Manzate Pro Stick T&O",

                    Uses = "Step 1: Determine how much Manzate Pro Stick T&O you need by first calculating the square footage of the area you will be treating. To do this, measure (in feet) and multiply the area length times the width (length x width = square footage). " +
                    "Depending on the disease, you will use 4 to 8 oz. of Manzate Pro-Stick T&O per 3-5 gallons of water per 1,000 sq. ft. of turfgrass. For ornamentals, the rate is 1 to 2 lbs. of product per 5 gallons of water per acre or 1 to 2 lbs. of product per 100 gallons of water." +
                    "\r\nStep 2: In a sprayer of your choice, add half the amount of water needed for the application, followed by the appropriate amount of Manzate Pro-Stick T&O based on your calculations. Fill with the remaining half of water and agitate well to ensure the product is properly mixed." +
                    "\r\nStep 3: Spray the fungicide onto the leaf of the plant, placing the sprayer on a fan spray setting for even coverage. Repeat applications can be made at 10 to 21-day intervals if necessary.",
                    MedicineCategoryId = 3,
                    ThumbnailId = 8,
                    

                },
                new Medicine
                {
                    MedicineId = 3,
                    Name = "pH Dat",

                    Uses = "- Use in case the soil pH fluctuates below the required level and harms plants." +
                    "\r\n- Use in the stage of soil preparation, before flowering and after harvest." +
                    "\r\n- Industrial plants, fruit trees: Mix 1 liter with 200 liters of water, water 2-4 liters for 1 root." +
                    "\r\n- Rice, vegetables: 1 liter/1000m2, dilute and water evenly on the soil.",
                    MedicineCategoryId = 2,
                    ThumbnailId = 9,
                    

                },
                new Medicine
                {
                    MedicineId = 4,
                    Name = "Ametox (Abamectin 1.8%)",

                    Uses = "Abamectin is a mixture of avermectins containing about 80% avermectin B1a and 20% avermectin B1b. " +
                    "These two components B1a and B1b have very similar biological and toxicological properties. " +
                    "The avermectins are insecticidal/miticidal compounds derived from the soil bacterium Streptomyces avermitilis. " +
                    "Abamectin is a natural fermentation product of this bacterium. It acts as an insecticide by affecting the nervous system of and paralyzing insects." +
                    " Abamectin is used to control insect and mite pests of citrus pear and nut tree crops and it is used by homeowners for control of fire ants.",
                    MedicineCategoryId = 1,
                    ThumbnailId = 14,
                
                },
                new Medicine
                {
                    MedicineId = 5,
                    Name = "Healthy Leaf",
                    Uses = "None",
                    MedicineCategoryId = 4,
                    ThumbnailId = 14,

                }
            );

            #endregion

            #region Treatment seed
            modelBuilder.Entity<Treatment>().HasData(
                new Treatment { TreatmentId = 1, 
                TreatmentName = "Remove Infected Leaves and  Soil Improvement", 

                Method = "Remove infected leaves during the growing season and remove all " +
                "infected plant parts at the end of the season. Prune or stake plants to improve air circulation and reduce fungal problems." +
                "\r\nMake sure to disinfect your pruning shears (one part bleach to 4 parts water) after each cut." +
                "Drip irrigation and soaker hoses can be used to help keep the foliage dry.\r\nFor best control, apply copper-based fungicides early," +
                " two weeks before disease normally appears or when weather forecasts predict a long period of wet weather. " +
                "Alternatively, begin treatment when disease first appears, and repeat every 7-10 days for as long as needed.", 
                ThumbnailId = 10,
                    
                },
                new Treatment { TreatmentId = 2, 
                TreatmentName = "Pulling out weeds",

                Method = "Eliminate initial source of infection by removing infected plant debris and weeds, and use disease-free seeds." +
                "\r\nIf complete removal of plant debris is not possible, destroy by deep plowing immediately after harvest and follow with a one-year rotation" +
                " with non-solanaceous crop.",
                ThumbnailId = 11,
                    
                },
                new Treatment { TreatmentId = 3,
                TreatmentName = "Add Magnesium for Plants",

                Method = "Tomatoes that don’t have enough magnesium will develop yellow leaves with green veins. If you’re sure of a magnesium deficiency," +
                " try a homemade Epsom salt mixture. Combine two tablespoons of Epsom salt with a gallon of water and spray the mixture on the plant.", 
                ThumbnailId = 12,
                    
                },
                new Treatment
                {
                    TreatmentId = 4,
                    TreatmentName = "Neem oil or Insecticidal soap",

                    Method = "Use natural sprays like neem oil or insecticidal soap to wash away aphids." +
                    "\r\n- Add two tablespoons of castile soap into one gallon of water, then spray aphids directly." +
                    "\r\n- Add two teaspoons of neem oil to one gallon of water, then spray aphids directly." +
                    "\r\nBe sure to cover both the top and bottom of leaves when spraying aphids. This will help ensure that you get all the aphids on your plants." +
                    "\r\nApply yellow stick boards around your tomato plants to attract aphids. Aphids are attracted to yellow and will travel towards it, then stick inside its glue coating.",
                    ThumbnailId = 12,

                },
                 new Treatment
                 {
                     TreatmentId = 5,
                     TreatmentName = "Healthy Leaf",
                     Method = "None",
                     ThumbnailId = 12,

                 }

            );
            #endregion


        }
    }
}
