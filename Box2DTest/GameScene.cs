using System;
using CocosSharp;
using Box2D.Common;
using Box2D.Dynamics;
using Box2D.Collision.Shapes;

namespace Box2DTest.Android
{
    public class GameScene : CCLayerColor
    {
        //box2d stuff
        const int PTM_RATIO = 32;
        b2World world;
        b2Body ball;
        b2Body ball2;
        b2Body ball3;
        CCPhysicsSprite sprite;
        CCPhysicsSprite sprite2;
        CCPhysicsSprite sprite3;
        float ballmass;
        float ball2mass;
        float ball3mass;
        const float Gvalue = -500f;
        const float repulfactor = 1f;
        const float repuldist = 10f;

        public GameScene()
        {
            Color = CCColor3B.Blue;
            Opacity = 255;
        }

        protected override void AddedToScene()
        {
            base.AddedToScene();
            Scene.SceneResolutionPolicy = CCSceneResolutionPolicy.ShowAll;


            StartScheduling();

            var label = new CCLabel("HelloWorld", "helvetica", 52f){ Position = VisibleBoundsWorldspace.Center };
            AddChild(label);

           
        }


        public static CCScene GameSceneLayer(CCWindow mainWindow)
        {
            var layer = new GameScene();
            var scene = new CCScene(mainWindow);
            scene.AddChild(layer);
            return scene;
        }


        void InitPhysics ()
        {
            CCSize s = Layer.VisibleBoundsWorldspace.Size;

            //world def
            var gravity = new b2Vec2 (0.0f, 0.0f);
            world = new b2World (gravity);
            world.SetAllowSleeping (true);
            world.SetContinuousPhysics (true);

            //body def
            var balldef = new b2BodyDef();
            balldef.type = b2BodyType.b2_dynamicBody;
            balldef.position.Set(s.Width *0.5f/ PTM_RATIO, s.Height * 0.8f / PTM_RATIO);
            ball = world.CreateBody(balldef);

            //shape def
            b2CircleShape ballshape = new b2CircleShape();
            ballshape.Radius = 50f / PTM_RATIO;

            //fixture def
            b2FixtureDef ballfixture = new b2FixtureDef();
            ballfixture.shape = ballshape;
            ballfixture.density = 1f;
            ballfixture.friction = 0.2f;
            ballfixture.restitution = 0.0f;
            ball.CreateFixture(ballfixture);
            //ball.SetTransform(new b2Vec2(8, 8), 1);  //this doesn't work
            ball.LinearVelocity = new b2Vec2(0, 10);
            //ball.AngularVelocity = 1;  //CCPhysicsSprite doesn't update angle

            //sprite def
            sprite = new CCPhysicsSprite(new CCTexture2D("ball.png"), new CCRect(0, 0, 100, 100), PTM_RATIO);
            sprite.Position = new CCPoint(balldef.position.x, balldef.position.y);
            sprite.PhysicsBody = ball;
            AddChild(sprite);


            /////////////////////  ball 2
            var fixedballdef = new b2BodyDef();
            fixedballdef.type = b2BodyType.b2_staticBody;
            fixedballdef.position.Set(s.Width * 0.5f / PTM_RATIO, s.Height * 0.5f / PTM_RATIO);
            ball2 = world.CreateBody(fixedballdef);
            ball2.CreateFixture(ballfixture);
            sprite2 = new CCPhysicsSprite(new CCTexture2D("ball.png"), new CCRect(0, 0, 100, 100), PTM_RATIO);
            sprite2.Position = new CCPoint(fixedballdef.position.x, fixedballdef.position.y);
            sprite2.PhysicsBody = ball2;
            AddChild(sprite2);

            /////////////////////  ball 3
            fixedballdef.position.Set(s.Width * 0.75f / PTM_RATIO, s.Height * 0.75f / PTM_RATIO);
            ball3 = world.CreateBody(fixedballdef);
            ball3.CreateFixture(ballfixture);
            sprite3 = new CCPhysicsSprite(new CCTexture2D("ball.png"), new CCRect(0, 0, 100, 100), PTM_RATIO);
            sprite3.Position = new CCPoint(fixedballdef.position.x, fixedballdef.position.y);
            sprite3.PhysicsBody = ball3;
            AddChild(sprite3);




            ballmass = ball.Mass;
            ball2mass = ball2.Mass;
            ball3mass = ball3.Mass;


            //put walls up
            var edge = new b2EdgeShape();
            edge.Set(new b2Vec2(0f, s.Height / PTM_RATIO), new b2Vec2(s.Width / PTM_RATIO, s.Height / PTM_RATIO));
            var wallfixture = new b2FixtureDef();
            wallfixture.shape = edge;
            var walldef = new b2BodyDef();
            walldef.type = b2BodyType.b2_staticBody;
            var wall = world.CreateBody(walldef);
            wall.CreateFixture(wallfixture);

            edge.Set(new b2Vec2(s.Width / PTM_RATIO, s.Height / PTM_RATIO), new b2Vec2(s.Width / PTM_RATIO, 0f));
            var wall2 = world.CreateBody(walldef);
            wall2.CreateFixture(wallfixture);

            edge.Set(new b2Vec2(0f, 0f), new b2Vec2(s.Width / PTM_RATIO, 0f));
            var wall3 = world.CreateBody(walldef);
            wall3.CreateFixture(wallfixture);

            edge.Set(new b2Vec2(0f, 0f), new b2Vec2(0f, s.Height / PTM_RATIO));
            var wall4 = world.CreateBody(walldef);
            wall4.CreateFixture(wallfixture);

//            Console.WriteLine("position of body ball2 {0}", ball2.Position);
//            Console.WriteLine("position of body sprite2 {0}", sprite2.Position);
//            Console.WriteLine("anchor point of body sprite {0}", sprite.AnchorPoint);
//            Console.WriteLine("content size of sprite {0}", sprite.ContentSize);
//            Console.WriteLine("content size of sprite2 {0}", sprite2.ContentSize);

            //sprite.UpdateBallTransform();

//            var def = new b2BodyDef ();
//            def.allowSleep = true;
//            def.position = b2Vec2.Zero;
//            def.type = b2BodyType.b2_staticBody;
//            b2Body groundBody = world.CreateBody (def);
//            groundBody.SetActive (true);
//
//            b2EdgeShape groundBox = new b2EdgeShape ();
//            groundBox.Set (b2Vec2.Zero, new b2Vec2 (s.Width / PTM_RATIO, 0));
//            b2FixtureDef fd = new b2FixtureDef ();
//            fd.shape = groundBox;
//            groundBody.CreateFixture (fd);
        }

        void StartScheduling()
        {
            Schedule (t => {
                world.Step (t, 6, 2);

                ball2mass = ballmass;
                var force = new b2Vec2(ball.Position.x - ball2.Position.x, ball.Position.y - ball2.Position.y);
                float dist = force.Length;
                force = (force/dist) * Gvalue * ballmass * ball2mass / (dist * dist);
                if (force.Length < 0.0000001f)
                {
                    force = b2Vec2.Zero;
                }

                //weak attractive force
                ball.ApplyForce(force,ball.WorldCenter);


                //strong repulsive force
                float range = dist / repuldist;

                ball.ApplyForce(-force * repulfactor / (range*range),ball.WorldCenter);
                //ball2.ApplyForce(force * repulfactor / (float)Math.Pow(range,3),ball.WorldCenter);

                //friction
                ball.LinearDamping = 1f;


                sprite.UpdateBallTransform();
                sprite2.UpdateBallTransform();
                sprite3.UpdateBallTransform();
              
//                foreach (CCPhysicsSprite sprite in ballsBatch.Children) {
//                    if (sprite.Visible && sprite.PhysicsBody.Position.x < 0f || sprite.PhysicsBody.Position.x * PTM_RATIO > ContentSize.Width) { //or should it be Layer.VisibleBoundsWorldspace.Size.Width
//                        world.DestroyBody (sprite.PhysicsBody);
//                        sprite.Visible = false;
//                        sprite.RemoveFromParent ();
//                    } else {
//                        sprite.UpdateBallTransform();
//                    }
//                }
            });
        }

        public override void OnEnter ()
        {
            base.OnEnter ();

            InitPhysics ();
            //ball.ApplyLinearImpulse(new b2Vec2(0f, -180f), ball.WorldCenter);
        }
    }
}

