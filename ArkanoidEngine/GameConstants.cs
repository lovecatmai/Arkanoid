namespace ArkanoidEngine
{
    /// <summary>
    /// Все константы игрового движка.
    /// </summary>
    public static class GameConstants
    {
        // consts that exists in more than one file


        /// <summary>Делитель половины ширины.</summary>
        public const float paddleHalfWidthDivisor = 2f;


        // GameEngine consts


        /// <summary>Минимальный счёт игрока.</summary>
        public const int minPlayerScore = 0;

        /// <summary>Скорость мяча в секунду.</summary>
        public const float ballSpeedPixelsPerSecond = 320f;

        /// <summary>Ширина платформы.</summary>
        public const float paddleWidthInPixels = 100f;

        /// <summary>Высота платформы.</summary>
        public const float paddleHeightInPixels = 14f;

        /// <summary>Отступ платформы от низа.</summary>
        public const float paddleOffsetFromBottom = 50f;

        /// <summary>Отступ мяча над платформой.</summary>
        public const float ballOffsetAbovePaddle = 18f;

        /// <summary>Радиус мяча.</summary>
        public const float ballRadiusInPixels = 8f;

        /// <summary>Урон мяча за удар.</summary>
        public const int ballDamagePerHit = 1;

        /// <summary>Начальная горизонтальная скорость мяча.</summary>
        public const float ballInitialHorizontalVelocity = 0f;

        /// <summary>Делитель половины ширины поля.</summary>
        public const float fieldHalfWidthDivisor = 2f;

        /// <summary>Левая граница поля.</summary>
        public const float wallLeftBoundary = 0f;

        /// <summary>Верхняя граница поля.</summary>
        public const float wallTopBoundary = 0f;

        /// <summary>Отступ мяча от залипания.</summary>
        public const float ballAntiStickOffsetFromPaddle = 1f;

        /// <summary>Множитель обратного направления.</summary>
        public const float negativeDirectionMultiplier = -1f;

        /// <summary>Очки за уничтоженный кирпич.</summary>
        public const int scorePerDestroyedBrick = 10;

        /// <summary>Ширина окна.</summary>
        public const int ClientSizeWindowWidth = 600;

        /// <summary>Высота окна.</summary>
        public const int ClientSizeWindowHeight = 800;


        // CollisionDetector consts


        /// <summary>Минимальный коэффициент удара.</summary>
        public const float hitRatioMinClamp = -1f;

        /// <summary>Максимальный коэффициент удара.</summary>
        public const float hitRatioMaxClamp = 1f;

        /// <summary>Максимальный угол отклонения.</summary>
        public const float maxDeflectionAngleInDegrees = 65f;

        /// <summary>Делитель градусов в радианы.</summary>
        public const float degreesToRadiansConversionDivisor = 180f;


        // LevelBuilder consts


        /// <summary>Количество колонок кирпичей.</summary>
        public const int numberOfColumns = 10;

        /// <summary>Количество рядов кирпичей.</summary>
        public const int numberOfRows = 6;

        /// <summary>Высота кирпича.</summary>
        public const float brickHeightInPixels = 20f;

        /// <summary>Горизонтальный зазор между кирпичами.</summary>
        public const float horizontalGapBetweenBricks = 3f;

        /// <summary>Вертикальный зазор между кирпичами.</summary>
        public const float verticalGapBetweenBricks = 5f;

        /// <summary>Отступ до первого ряда.</summary>
        public const float topOffsetBeforeFirstRow = 70f;

        /// <summary>Отступ от боковых стен.</summary>
        public const float sideMarginFromWall = 4f;

        /// <summary>Количество зазоров между колонками.</summary>
        public const int numberOfGapsBetweenColumns = numberOfColumns - 1;

        /// <summary>Количество боковых отступов.</summary>
        public const int numberOfSideMargins = 2;

        /// <summary>HP кирпичей первого уровня.</summary>
        public const int brickHitPointsForLevel1 = 1;

        /// <summary>Начальный индекс колонки.</summary>
        public const int firstColumnIndex = 0;

        /// <summary>Начальный индекс ряда.</summary>
        public const int firstRowIndex = 0;


        // Ball consts


        /// <summary>Радиус мяча.</summary>
        public const float ballRadius = 8f;

        /// <summary>Урон мяча.</summary>
        public const int ballDamage = 1;

        // Brick consts
        /// <summary>Минимальное HP кирпича.</summary>
        public const int brickMinHP = 0;

        /// <summary>Максимальное HP кирпича.</summary>
        public const int brickMaxHP = 1;

        /// <summary>Делитель половины ширины.</summary>
        public const float halfWidthDivisor = 2f;

        /// <summary>Делитель половины высоты.</summary>
        public const float halfHeightDivisor = 2f;

        // Paddle consts
        /// <summary>Ширина платформы.</summary>
        public const float paddleWidth = 100f;

        /// <summary>Высота платформы.</summary>
        public const float paddleHeigth = 15f;

        /// <summary>Делитель пополам.</summary>
        public const float halfDivisor = 2f;
    }
}